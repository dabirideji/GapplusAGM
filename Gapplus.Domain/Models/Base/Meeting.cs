using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Gapplus.Domain.Models.Base
{
    public class Meeting
    {
        [Key]
        public Guid MeetingId { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public string? MeetingDetails { get; set; }
        public string? MeetingStatus { get; set; }
        public DateTime MeetingCreatedAt { get; set; }
        public DateTime MeetingUpdatedAt { get; set; }
    }

    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }
        public String CompanyName { get; set; }
        public String CompanyDescription { get; set; }
        public String CompanyImageUrl { get; set; }
        public String CompanyRegNo { get; set; }
        public List<String> Tags { get; set; }
        public string? CompanyStatus { get; set; }
        public DateTime CompanyCreatedAt { get; set; }
        public DateTime CompanyUpdatedAt { get; set; }
    }




    [PrimaryKey("ShareHolderId", "CompanyId")]
    public class ShareHolderCompanyRelationShip
    {

        public Guid ShareHolderId { get; set; }
        public Guid CompanyId { get; set; }
    }


    [PrimaryKey("ShareHolderId", "MeetingId")]
    public class MeetingRegistration
    {

        public Guid ShareHolderId { get; set; }
        public Guid MeetingId { get; set; }
    }
}