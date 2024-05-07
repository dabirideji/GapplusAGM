using BarcodeGenerator.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BarcodeGenerator.Hubs
{
    public class FakeResolutionHub : Hub
    {
        public async Task SendResolutionStarted(FakeResolutionModel resolution)
        {
            await Clients.All.SendAsync("resolutionStarted",resolution);
        }
    }

    // public class FakeResolutionDbContext:DbContext {
    //     public FakeResolutionDbContext(DbContextOptions<FakeResolutionDbContext> options):base(options)
    //     {}
    //     public DbSet<FakeResolutionModel> FakeResolutions {get;set;}
    // }
}