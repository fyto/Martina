using Martina.API.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Care> Cares { get; set; }

        public DbSet<Disease> Deseases { get; set; }

        public DbSet<DiseaseType> DeseaseTypes { get; set; }

     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Care>().HasIndex(x => x.Description).IsUnique();    
            modelBuilder.Entity<Disease>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<DiseaseType>().HasIndex(x => x.Description).IsUnique();
        }

    }
}
