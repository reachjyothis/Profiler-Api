// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Profiler_Api.Data;

#nullable disable

namespace ProfilerApi.Data.Migrations
{
    [DbContext(typeof(SchedulerContext))]
    partial class SchedulerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Profiler_Api.DbModels.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Date = new DateTime(2023, 2, 2, 2, 56, 5, 323, DateTimeKind.Local).AddTicks(725),
                            Email = "admin@localhost.com",
                            PasswordHash = "1D57743E41C84192A747A6A2F692FC97B5A2C3D1DB7CDCA328463B85FFB7DBF2457C419486640E458D0EC1664AA2E80EA0E7289F8EF79DBEE9DF1BCCDF2388A7",
                            PasswordSalt = "4C501A810366409E230AECB8A57D1DB01F834DDDA53416B32D1E7C9EDB9A7DB8B07932EFA3FD773552BCFE097426E713E1D3D525E5B632D3447C0A91EC4860D6",
                            Role = "A",
                            Username = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
