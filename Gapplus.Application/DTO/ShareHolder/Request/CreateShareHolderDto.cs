using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gapplus.Application.DTO.ShareHolder.Request
{
    public class CreateShareHolderDto
    {
        
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string emailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }


    }

    public class UpdateShareHolderDto
    {
        
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }


    }
}