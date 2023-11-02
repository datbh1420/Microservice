﻿// <auto-generated />
using BackEnd.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackEnd.ProductAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.3.23174.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BackEnd.ProductAPI.Models.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageLocalPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("products");

                    b.HasData(
                        new
                        {
                            Id = "c484e233-89c5-4dd0-9759-948a09a92682",
                            CategoryName = "Stuffed animal",
                            Description = "Tired, tense, stressed,...after every hour of studying, taking exams, working??? Let To relieve your tired feeling with these extremely funny and cute elastic toys!",
                            ImageLocalPath = "Images\\CriticChickens.png",
                            ImagePath = "https://localhost:7001/Images/CriticChickens.png",
                            Name = "STRESS RELEASING ELASTIC TOYS CUTE CRITIC CHICKENS",
                            Price = 68000.0,
                            Quantity = 100
                        },
                        new
                        {
                            Id = "956192cb-d2a2-4957-8c4a-3500a536019c",
                            CategoryName = "Lego",
                            Description = "Nowadays, instead of playing with iPads, cell phones, watching TV, etc., which increases the possibility of screen light harming your eyes, playing with educational toys will help you both relax and Develop thinking ability and creativity!",
                            ImageLocalPath = "Images\\AstronomyLego.png",
                            ImagePath = "https://localhost:7001/Images/AstronomyLego.png",
                            Name = "LEGO ASTRONOMY IN FLY",
                            Price = 135000.0,
                            Quantity = 200
                        },
                        new
                        {
                            Id = "65c37ebd-95ab-429b-844c-1e230d1a14ec",
                            CategoryName = "Keychain",
                            Description = "Keychains are a more convenient key decoration and hanging product, helping you show off the difference in your key cluster. Using a key hanger will bring you more convenience and ease when storing and carrying with you, limiting misplacement or loss.",
                            ImageLocalPath = "Images\\ShibaHipHop.png",
                            ImagePath = "https://localhost:7001/Images/ShibaHiphop.png",
                            Name = "SHIBA HIPHOP FLEXIBLE KEYCHAIN",
                            Price = 35000.0,
                            Quantity = 300
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
