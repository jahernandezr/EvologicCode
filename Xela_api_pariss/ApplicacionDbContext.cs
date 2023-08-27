using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Xela_api_pariss.Entidades;
using Xela_api_pariss.Models;

namespace Xela_api_pariss

{
    public class ApplicacionDbContext : IdentityDbContext
   //      public class ApplicacionDbContext : DbContext
    {
         public ApplicacionDbContext(DbContextOptions<ApplicacionDbContext> options) : base(options) { 
         }
        protected override void OnModelCreating(ModelBuilder  modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        //public DbSet<VwCTCDigitalizations> VwCTCDigitalizations { get; set; }
    }
  }

