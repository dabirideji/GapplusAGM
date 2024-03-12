using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gapplus.Infrastructure.Data
{
    public class GapplusDbContext : DbContext
    {
        public GapplusDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}