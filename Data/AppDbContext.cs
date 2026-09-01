using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyICSv2._1.Models;

namespace MyICSv2._1.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Device> Devices => Set<Device>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Devices");
                entity.HasKey(i => i.DeviceId);
                entity.Property(i => i.DeviceName)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(i => i.DeviceAddressIPv4)
                      .IsRequired()
                      .HasMaxLength(12);
                entity.Property(i => i.DeviceAddressIPv6)
                      .HasMaxLength(39);
                entity.Property(i => i.IsActive);
                entity.Property(i => i.DeviceStatus)
                      .HasMaxLength(20);
            });
        }
    }
}
