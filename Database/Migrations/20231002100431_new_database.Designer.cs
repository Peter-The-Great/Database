﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20231002100431_new_database")]
    partial class new_database
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Database.Attractie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Attracties");
                });

            modelBuilder.Entity("Database.GastInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LaatstBezochteURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GastInfo");
                });

            modelBuilder.Entity("Database.Gebruiker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Gebruiker");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Database.Onderhoud", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttractieId")
                        .HasColumnType("int");

                    b.Property<string>("Probleem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AttractieId");

                    b.ToTable("Onderhoud");
                });

            modelBuilder.Entity("Database.Reservering", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttractieId")
                        .HasColumnType("int");

                    b.Property<int?>("GastId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttractieId");

                    b.HasIndex("GastId");

                    b.ToTable("Reserveringen");
                });

            modelBuilder.Entity("MedewerkerOnderhoud", b =>
                {
                    b.Property<int>("CoordinatorsId")
                        .HasColumnType("int");

                    b.Property<int>("CoordineertId")
                        .HasColumnType("int");

                    b.HasKey("CoordinatorsId", "CoordineertId");

                    b.HasIndex("CoordineertId");

                    b.ToTable("MedewerkerOnderhoud");
                });

            modelBuilder.Entity("MedewerkerOnderhoud1", b =>
                {
                    b.Property<int>("DoetId")
                        .HasColumnType("int");

                    b.Property<int>("MedewerkerId")
                        .HasColumnType("int");

                    b.HasKey("DoetId", "MedewerkerId");

                    b.HasIndex("MedewerkerId");

                    b.ToTable("MedewerkerOnderhoud1");
                });

            modelBuilder.Entity("Database.Gast", b =>
                {
                    b.HasBaseType("Database.Gebruiker");

                    b.Property<int?>("BegeleiderId")
                        .HasColumnType("int");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<DateTime>("EersteBezoek")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FavorietId")
                        .HasColumnType("int");

                    b.Property<int>("GastInfoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("GeboorteDatum")
                        .HasColumnType("datetime2");

                    b.HasIndex("BegeleiderId");

                    b.HasIndex("FavorietId");

                    b.HasIndex("GastInfoId")
                        .IsUnique()
                        .HasFilter("[GastInfoId] IS NOT NULL");

                    b.ToTable("Gast");
                });

            modelBuilder.Entity("Database.Medewerker", b =>
                {
                    b.HasBaseType("Database.Gebruiker");

                    b.ToTable("Medewerker");
                });

            modelBuilder.Entity("Database.GastInfo", b =>
                {
                    b.OwnsOne("Database.Coordinaat", "Coordinaat", b1 =>
                        {
                            b1.Property<int>("GastInfoId")
                                .HasColumnType("int");

                            b1.Property<int>("x")
                                .HasColumnType("int");

                            b1.Property<int>("y")
                                .HasColumnType("int");

                            b1.HasKey("GastInfoId");

                            b1.ToTable("GastInfo");

                            b1.WithOwner()
                                .HasForeignKey("GastInfoId");
                        });

                    b.Navigation("Coordinaat")
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Onderhoud", b =>
                {
                    b.HasOne("Database.Attractie", "Attractie")
                        .WithMany("Onderhoud")
                        .HasForeignKey("AttractieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Database.DateTimeBereik", "DateTimeBereik", b1 =>
                        {
                            b1.Property<int>("OnderhoudId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Begin")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("Eind")
                                .HasColumnType("datetime2");

                            b1.HasKey("OnderhoudId");

                            b1.ToTable("Onderhoud");

                            b1.WithOwner()
                                .HasForeignKey("OnderhoudId");
                        });

                    b.Navigation("Attractie");

                    b.Navigation("DateTimeBereik")
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Reservering", b =>
                {
                    b.HasOne("Database.Attractie", "Attractie")
                        .WithMany("Reserveringen")
                        .HasForeignKey("AttractieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Gast", "Gast")
                        .WithMany("Reserveringen")
                        .HasForeignKey("GastId");

                    b.OwnsOne("Database.DateTimeBereik", "DateTimeBereik", b1 =>
                        {
                            b1.Property<int>("ReserveringId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Begin")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("Eind")
                                .HasColumnType("datetime2");

                            b1.HasKey("ReserveringId");

                            b1.ToTable("Reserveringen");

                            b1.WithOwner()
                                .HasForeignKey("ReserveringId");
                        });

                    b.Navigation("Attractie");

                    b.Navigation("DateTimeBereik")
                        .IsRequired();

                    b.Navigation("Gast");
                });

            modelBuilder.Entity("MedewerkerOnderhoud", b =>
                {
                    b.HasOne("Database.Medewerker", null)
                        .WithMany()
                        .HasForeignKey("CoordinatorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Onderhoud", null)
                        .WithMany()
                        .HasForeignKey("CoordineertId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedewerkerOnderhoud1", b =>
                {
                    b.HasOne("Database.Onderhoud", null)
                        .WithMany()
                        .HasForeignKey("DoetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Medewerker", null)
                        .WithMany()
                        .HasForeignKey("MedewerkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Gast", b =>
                {
                    b.HasOne("Database.Gast", "Begeleider")
                        .WithMany()
                        .HasForeignKey("BegeleiderId");

                    b.HasOne("Database.Attractie", "Favoriet")
                        .WithMany()
                        .HasForeignKey("FavorietId");

                    b.HasOne("Database.GastInfo", "GastInfo")
                        .WithOne("Gast")
                        .HasForeignKey("Database.Gast", "GastInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Gebruiker", null)
                        .WithOne()
                        .HasForeignKey("Database.Gast", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Begeleider");

                    b.Navigation("Favoriet");

                    b.Navigation("GastInfo");
                });

            modelBuilder.Entity("Database.Medewerker", b =>
                {
                    b.HasOne("Database.Gebruiker", null)
                        .WithOne()
                        .HasForeignKey("Database.Medewerker", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Attractie", b =>
                {
                    b.Navigation("Onderhoud");

                    b.Navigation("Reserveringen");
                });

            modelBuilder.Entity("Database.GastInfo", b =>
                {
                    b.Navigation("Gast")
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Gast", b =>
                {
                    b.Navigation("Reserveringen");
                });
#pragma warning restore 612, 618
        }
    }
}
