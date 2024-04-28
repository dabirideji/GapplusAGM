using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain;
using Gapplus.Domain.Categories;

namespace Gapplus.Application.DTO.User.Response
{
    public class ReadUserDto
    {
        public Guid UserId { get; set; }
        public String? UserFirstName { get; set; }
        public String? UserLastName { get; set; }
        public String? UserName { get; set; }
        public String? UserEmail { get; set; }
        public String? UserPhoneNumber { get; set; }
        public String? UserPassword { get; set; }
        public UserStatus UserStatus { get; set; }
        public String? UserCreatedAt { get; set; }
        public String? UserUpdatedAt { get; set; }

        // NAVIGATTON PROPERTY FOR THE USER COMPANY
        public Guid CompanyId { get; set; }
        // public Gapplus.Domain.Company? Company { get; set; }
    }
}
