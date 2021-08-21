using Martina.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<DiseaseType> DeseaseTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Brand>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<DiseaseType>().HasIndex(x => x.Description).IsUnique();
            //modelBuilder.Entity<Procedure>().HasIndex(x => x.Description).IsUnique();
            //modelBuilder.Entity<Vehicle>().HasIndex(x => x.Plaque).IsUnique();
            //modelBuilder.Entity<VehicleType>().HasIndex(x => x.Description).IsUnique();
        }

    }
}
