using Microsoft.EntityFrameworkCore;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Services.Implementation;

namespace Report_A_Crime.Context
{
    public class ReportCrimeDbContext : DbContext
    {
        public ReportCrimeDbContext(DbContextOptions<ReportCrimeDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Geolocation> Geolocations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<RequestAService> RequestsAService { get; set; }
        public DbSet<SharedWithUs> SharedWithUs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RequestAService>().HasKey(r => r.RequestServiceId);

            modelBuilder.Entity<Report>().Property(e => e.CreatedAt).HasConversion(v => v, v => DateTime.SpecifyKind(v,DateTimeKind.Utc));

            modelBuilder.Entity<Report>().Property(e => e.DateOccurred).HasConversion(v => v, v => DateTime.SpecifyKind(v,DateTimeKind.Utc));


            modelBuilder.Entity<SharedWithUs>().HasKey(s => s.ShareWithUsId);

            modelBuilder.Entity<Role>().HasData(new Role
            {
                RoleId = new Guid("6b541cc9-b08b-47d3-b52f-3ca6aa06a1e6"),
                RoleName = "Admin",
            });
            modelBuilder.Entity<Role>().HasData(new Role
            {
                RoleId = new Guid("0504ea46-35aa-4949-9c59-b8b32a083ef6"),
                RoleName = "User",
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                RoleId = new Guid("6b541cc9-b08b-47d3-b52f-3ca6aa06a1e6"),
                UserName = "Admin",
                HashSalt = "Admin",
                PhoneNumber = "07011208687",
                Email = "admin@yahoomail.com",
                IsAnonymous = false,
                KycStatus = true,
            });

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Reports)
            //    .WithOne(r => r.User)
            //    .HasForeignKey(r => r.UserId);

            //modelBuilder.Entity<User>()
            //   .HasMany(u => u.SharedWithUs)
            //   .WithOne(s => s.User)
            //   .HasForeignKey(s => s.UserId);

            //modelBuilder.Entity<Role>()
            //  .HasMany(r => r.Users)
            //  .WithOne(u => u.Role);

        }
    }
}
