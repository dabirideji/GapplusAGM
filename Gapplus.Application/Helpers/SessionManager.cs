using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessionManager
{
    private static IHttpContextAccessor _httpContextAccessor;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


 public static void ClearAllSessionData()
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            httpContext.Session.Clear();
        }
        else
        {
            throw new Exception("SESSION NOT AVAIILABLE || SESSION COULD NOT BE ACCESSED");

            // Handle the case when HttpContext is not available
        }
    }
    public static void ClearSessionData(string key)
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            httpContext.Session.Remove(key);
        }
        else
        {
            throw new Exception("SESSION NOT AVAIILABLE || SESSION COULD NOT BE ACCESSED");
            // Handle the case when HttpContext is not available
        }
    }

    public static void SetSessionData(string key, object value)
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            string jsonString = JsonConvert.SerializeObject(value);
            httpContext.Session.SetString(key, jsonString);
        }
        else
        {
            // Handle the case when HttpContext is not available
        }
    }



    public static T GetSessionData<T>(string key) where T : class
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            var jsonString = httpContext.Session.GetString(key);
            if(jsonString==null){
                return null;
            }
            var jsonData = JsonConvert.DeserializeObject<T>(jsonString);
            return jsonData;
        }
        else
        {
            // Handle the case when HttpContext is not available
            return null;
        }
    }
}

public static class SessionInitializer
{
    public static void Initialize(IHttpContextAccessor httpContextAccessor)
    {
        SessionManager.Configure(httpContextAccessor);
    }
}
