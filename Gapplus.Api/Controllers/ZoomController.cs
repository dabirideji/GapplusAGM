using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Gapplus.Application.DTO.ZoomMeeting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class ZoomController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZoomController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("GetToken")]
    public async Task<ActionResult<string>> GetTokenAsync()
    {
        string tokenEndpoint = "https://zoom.us/oauth/token";
        string accountId = "tCSrlCERQRKkXHT_9ysm7A";
        string clientId = "jkcFJkHT2qhGZ77MGaiDw";
        string clientSecret = "L7sPg2FzsnAE4P0MGbDDzRZII9u5b6X3";

        var requestBody = $"grant_type=account_credentials&account_id={accountId}";

        using (HttpClient client = new HttpClient())
        {
            var credentials = $"{clientId}:{clientSecret}";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(tokenEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);
                return Ok(tokenResponse.access_token.Value);
            }
            else
            {
                return BadRequest("Failed to get access token");
            }
        }
    }

       [HttpGet("generateSignature")]
        public IActionResult GenerateJwt(string key, string secret, string meetingNumber, int role)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                var claims = new[]
                {
                    new Claim("appKey", key),
                    new Claim("sdkKey", key),
                    new Claim("mn", meetingNumber),
                    new Claim("role", role.ToString()),
                    new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new Claim("exp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString()),
                    new Claim("tokenExp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2), // Convert DateTimeOffset to DateTime
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);

                return Ok(new { token = jwt });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while generating the JWT." });
            }
        }

    //  [HttpGet("generateSignature")]
    //     public IActionResult GenerateJwt(string key, string secret, string meetingNumber, int role)
    //     {
    //         try
    //         {
    //             var tokenHandler = new JwtSecurityTokenHandler();
    //             var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

    //             var claims = new[]
    //             {
    //                 new Claim("appKey", key),
    //                 new Claim("sdkKey", key),
    //                 new Claim("mn", meetingNumber),
    //                 new Claim("role", role.ToString()),
    //                 new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
    //                 new Claim("exp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString()),
    //                 new Claim("tokenExp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString())
    //             };

    //             var tokenDescriptor = new SecurityTokenDescriptor
    //             {
    //                 Subject = new ClaimsIdentity(claims),
    //                 Expires = DateTimeOffset.UtcNow.AddHours(2),
    //                 SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
    //             };

    //             var token = tokenHandler.CreateToken(tokenDescriptor);
    //             var jwt = tokenHandler.WriteToken(token);

    //             return Ok(new { token = jwt });
    //         }
    //         catch (Exception ex)
    //         {
    //             return StatusCode(500, new { error = "An error occurred while generating the JWT." });
    //         }
    //     }

    [HttpPost("createMeeting")]
    public async Task<ActionResult<string>> CreateMeetingAsync(SimpleCreateMeetingDto meetingDto)
    {
        var token = await GetAccessTokenAsync();

        DateTime startTime = meetingDto.StartTimeInMinutes.HasValue ? DateTime.UtcNow.AddMinutes(meetingDto.StartTimeInMinutes.Value) : DateTime.UtcNow;

        var meetingRequest = new
        {
            topic = meetingDto.Topic,
            start_time = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            duration = meetingDto.Duration,
            timezone = "UTC",
            agenda = meetingDto.Agenda,
            settings = new
            {
                host_video = true,
                participant_video = true,
                join_before_host = true,
                mute_upon_entry = true,
                watermark = false,
                use_pmi = false,
                approval_type = 0,
                audio = "both",
                auto_recording = "none"
            }
        };

        var jsonMeeting = JsonConvert.SerializeObject(meetingRequest);
        var content = new StringContent(jsonMeeting, Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsync("https://api.zoom.us/v2/users/me/meetings", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }
        else
        {
            return BadRequest("Failed to create meeting");
        }
    }

     [HttpGet("meetings")]
    public async Task<ActionResult<ZoomMeetingsResponseDto>> GetAllMeetingsAsync([FromQuery] PaginationParams paginationParams)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAccessTokenAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var meetings = await FetchMeetingsAsync(client, paginationParams);

        // Return the aggregated list of meetings
        var zoomMeetingsResponse = new ZoomMeetingsResponseDto
        {
            PageSize = paginationParams.PageSize,
            TotalRecords = meetings.Count,
            NextPageToken = "", // Since all pages have been fetched
            Meetings = meetings
        };

        return Ok(zoomMeetingsResponse);
    }

    [HttpPost("GenerateZoomSignature")]
    private ActionResult<string> GenerateZoomSignature(string meetingNumber, int role)
    {
        string sdkKey = "jkcFJkHT2qhGZ77MGaiDw";
        string sdkSecret = "L7sPg2FzsnAE4P0MGbDDzRZII9u5b6X3";

        // Create JWT token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Convert secret key to bytes
        var keyBytes = Encoding.ASCII.GetBytes(sdkSecret);

        // Create token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("appKey", sdkKey),
                new Claim("sdkKey", sdkKey),
                new Claim("mn", meetingNumber),
                new Claim("role", role.ToString()),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim("exp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString()),
                new Claim("tokenExp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString())
            }),
            Expires = DateTimeOffset.UtcNow.AddHours(2).DateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        // Create JWT token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Write JWT token as a string
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(jwtToken);
    }

 [HttpDelete("meetings/{meetingId}")]
    public async Task<IActionResult> DeleteMeetingAsync(long meetingId)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAccessTokenAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync($"https://api.zoom.us/v2/meetings/{meetingId}");

        if (response.IsSuccessStatusCode)
        {
            return NoContent(); // Return 204 No Content upon successful deletion
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest($"Failed to delete meeting: {response.StatusCode}, {errorMessage}");
        }
    }

  [HttpPatch("meetings/{meetingId}")]
    public async Task<ActionResult<ZoomMeetingDto>> UpdateMeetingAsync(long meetingId, [FromBody] UpdateZoomMeetingDto meetingUpdate)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAccessTokenAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var json = JsonConvert.SerializeObject(meetingUpdate);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PatchAsync($"https://api.zoom.us/v2/meetings/{meetingId}", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedMeeting = JsonConvert.DeserializeObject<ZoomMeetingDto>(responseContent);
            return Ok(updatedMeeting);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest($"Failed to update meeting: {response.StatusCode}, {errorMessage}");
        }
    }

    private async Task<List<ZoomMeetingDto>> FetchMeetingsAsync(HttpClient client, PaginationParams paginationParams)
    {
        var meetings = new List<ZoomMeetingDto>();

        while (true)
        {
            var response = await client.GetAsync($"https://api.zoom.us/v2/users/me/meetings?page_size={paginationParams.PageSize}&page_number={paginationParams.PageNumber}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var meetingsResponse = JsonConvert.DeserializeObject<ZoomMeetingsResponseDto>(responseContent);

                // Add fetched meetings to the list
                meetings.AddRange(meetingsResponse.Meetings);

                // Check if there are more pages to fetch
                if (string.IsNullOrEmpty(meetingsResponse.NextPageToken))
                {
                    // No more pages, exit the loop
                    break;
                }
                else
                {
                    // Move to the next page
                    paginationParams.PageNumber++;
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to fetch meetings: {response.StatusCode}, {errorMessage}");
            }
        }

        return meetings;
    }


    [HttpGet("meetings/{meetingId}/join")]
    public async Task<ActionResult<string>> GetMeetingJoinUrlAsync(long meetingId)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAccessTokenAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"https://api.zoom.us/v2/meetings/{meetingId}");

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var meetingInfo = JsonConvert.DeserializeObject<ZoomMeetingDto>(responseContent);

            // Assuming the ZoomMeetingDto contains the join URL property
            return Ok(meetingInfo.JoinUrl);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest($"Failed to get meeting details: {response.StatusCode}, {errorMessage}");
        }
    }

    private async Task<string> GetAccessTokenAsync()
    {
        string tokenEndpoint = "https://zoom.us/oauth/token";
        string accountId = "tCSrlCERQRKkXHT_9ysm7A";
        string clientId = "jkcFJkHT2qhGZ77MGaiDw";
        string clientSecret = "L7sPg2FzsnAE4P0MGbDDzRZII9u5b6X3";

        var requestBody = $"grant_type=account_credentials&account_id={accountId}";

        using (HttpClient client = new HttpClient())
        {
            var credentials = $"{clientId}:{clientSecret}";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(tokenEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);
                return tokenResponse.access_token;
            }
            else
            {
                return null;
            }
        }
    }
}
