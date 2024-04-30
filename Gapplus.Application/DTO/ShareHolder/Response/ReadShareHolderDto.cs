using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain.Models.Base;

namespace Gapplus.Application.DTO.ShareHolder.Response
{
    public class ReadShareHolderDto
    {
    public Guid ShareHolderId { get; set; }
    public int ShareHolderNum { get; set; }
    public string Address { get; set; }
    public string emailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string Name {get; set;}
    public List<int>? Interests { get; set; }
    public ConsolidationStatus ConsolidationStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    }
}