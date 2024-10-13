﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Report_A_Crime.Context;

#nullable disable

namespace Report_A_Crime.Migrations
{
    [DbContext(typeof(ReportCrimeDbContext))]
    partial class ReportCrimeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CategoryDescription")
                        .HasColumnType("text");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateCategory")
                        .HasColumnType("boolean");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Geolocation", b =>
                {
                    b.Property<Guid>("GeolocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.HasKey("GeolocationId");

                    b.HasIndex("ReportId");

                    b.ToTable("Geolocations");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Report", b =>
                {
                    b.Property<Guid>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOccurred")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("HeightOfTheOffender")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameOfTheOffender")
                        .HasColumnType("text");

                    b.Property<string>("ReportDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<string>("UploadEvidenceUrl")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ReportId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.RequestAService", b =>
                {
                    b.Property<Guid>("RequestServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("RequestServiceId");

                    b.HasIndex("ReportId");

                    b.HasIndex("UserId");

                    b.ToTable("RequestsAService");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("6b541cc9-b08b-47d3-b52f-3ca6aa06a1e6"),
                            RoleName = "Admin"
                        },
                        new
                        {
                            RoleId = new Guid("0504ea46-35aa-4949-9c59-b8b32a083ef6"),
                            RoleName = "User"
                        });
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.SharedWithUs", b =>
                {
                    b.Property<Guid>("ShareWithUsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<string>("ShareWithUsDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ShareWithUsId");

                    b.HasIndex("UserId");

                    b.ToTable("SharedWithUs");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("HashSalt")
                        .HasColumnType("text");

                    b.Property<bool?>("IsAnonymous")
                        .HasColumnType("boolean");

                    b.Property<bool>("KycStatus")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                            Email = "admin@yahoomail.com",
                            HashSalt = "Admin",
                            IsAnonymous = false,
                            KycStatus = true,
                            PhoneNumber = "07011208687",
                            RoleId = new Guid("6b541cc9-b08b-47d3-b52f-3ca6aa06a1e6"),
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Geolocation", b =>
                {
                    b.HasOne("Report_A_Crime.Models.Entities.Report", "Reports")
                        .WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Report", b =>
                {
                    b.HasOne("Report_A_Crime.Models.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Report_A_Crime.Models.Entities.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.RequestAService", b =>
                {
                    b.HasOne("Report_A_Crime.Models.Entities.Report", "Reports")
                        .WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Report_A_Crime.Models.Entities.User", "User")
                        .WithMany("RequestAServices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reports");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.SharedWithUs", b =>
                {
                    b.HasOne("Report_A_Crime.Models.Entities.User", "User")
                        .WithMany("SharedWithUs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.User", b =>
                {
                    b.HasOne("Report_A_Crime.Models.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Report_A_Crime.Models.Entities.User", b =>
                {
                    b.Navigation("Reports");

                    b.Navigation("RequestAServices");

                    b.Navigation("SharedWithUs");
                });
#pragma warning restore 612, 618
        }
    }
}
