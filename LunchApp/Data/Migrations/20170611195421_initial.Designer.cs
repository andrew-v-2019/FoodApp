using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Data.Models;

namespace Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20170611195421_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Models.Menu", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("Editable");

                    b.Property<string>("FilePath");

                    b.Property<DateTime>("LunchDate");

                    b.Property<string>("Name");

                    b.Property<double?>("Price");

                    b.HasKey("MenuId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Data.Models.MenuItem", b =>
                {
                    b.Property<int>("MenuItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MenuId");

                    b.Property<int>("MenuSectionId");

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.HasKey("MenuItemId");

                    b.HasIndex("MenuId");

                    b.HasIndex("MenuSectionId");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("Data.Models.MenuSection", b =>
                {
                    b.Property<int>("MenuSectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.HasKey("MenuSectionId");

                    b.ToTable("MenuSections");
                });

            modelBuilder.Entity("Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompName");

                    b.Property<string>("Ip");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data.Models.UserLunch", b =>
                {
                    b.Property<int>("UserLunchId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("FilePath");

                    b.Property<int>("MenuId");

                    b.Property<DateTime>("SubmitionDate");

                    b.Property<bool>("Submitted");

                    b.Property<int>("UserId");

                    b.HasKey("UserLunchId");

                    b.HasIndex("MenuId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLunches");
                });

            modelBuilder.Entity("Data.Models.UserLunchItem", b =>
                {
                    b.Property<int>("UserLunchItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("MenuItemId");

                    b.Property<int>("UserLunchId");

                    b.HasKey("UserLunchItemId");

                    b.HasIndex("MenuItemId");

                    b.HasIndex("UserLunchId");

                    b.ToTable("UserLunchItems");
                });

            modelBuilder.Entity("Data.Models.MenuItem", b =>
                {
                    b.HasOne("Data.Models.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("MenuId");

                    b.HasOne("Data.Models.MenuSection", "MenuSection")
                        .WithMany()
                        .HasForeignKey("MenuSectionId");
                });

            modelBuilder.Entity("Data.Models.UserLunch", b =>
                {
                    b.HasOne("Data.Models.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("MenuId");

                    b.HasOne("Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Data.Models.UserLunchItem", b =>
                {
                    b.HasOne("Data.Models.MenuItem", "MenuItem")
                        .WithMany()
                        .HasForeignKey("MenuItemId");

                    b.HasOne("Data.Models.UserLunch", "UserLunch")
                        .WithMany("UserLunchItems")
                        .HasForeignKey("UserLunchId");
                });
        }
    }
}
