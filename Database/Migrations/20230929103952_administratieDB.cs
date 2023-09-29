using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class administratieDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attracties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attracties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GastInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coordinaat_x = table.Column<int>(type: "int", nullable: false),
                    Coordinaat_y = table.Column<int>(type: "int", nullable: false),
                    LaatstBezochteURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gebruiker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruiker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Onderhoud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeBereik_Begin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeBereik_Eind = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Probleem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttractieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onderhoud", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Onderhoud_Attracties_AttractieId",
                        column: x => x.AttractieId,
                        principalTable: "Attracties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BegeleiderId = table.Column<int>(type: "int", nullable: true),
                    GastInfoId = table.Column<int>(type: "int", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EersteBezoek = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FavorietId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gast_Attracties_FavorietId",
                        column: x => x.FavorietId,
                        principalTable: "Attracties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gast_GastInfo_GastInfoId",
                        column: x => x.GastInfoId,
                        principalTable: "GastInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gast_Gast_BegeleiderId",
                        column: x => x.BegeleiderId,
                        principalTable: "Gast",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gast_Gebruiker_Id",
                        column: x => x.Id,
                        principalTable: "Gebruiker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medewerker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medewerker_Gebruiker_Id",
                        column: x => x.Id,
                        principalTable: "Gebruiker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserveringen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeBereik_Begin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeBereik_Eind = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GastId = table.Column<int>(type: "int", nullable: true),
                    AttractieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Attracties_AttractieId",
                        column: x => x.AttractieId,
                        principalTable: "Attracties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Gast_GastId",
                        column: x => x.GastId,
                        principalTable: "Gast",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedewerkerOnderhoud",
                columns: table => new
                {
                    CoordinatorsId = table.Column<int>(type: "int", nullable: false),
                    CoordineertId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedewerkerOnderhoud", x => new { x.CoordinatorsId, x.CoordineertId });
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud_Medewerker_CoordinatorsId",
                        column: x => x.CoordinatorsId,
                        principalTable: "Medewerker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud_Onderhoud_CoordineertId",
                        column: x => x.CoordineertId,
                        principalTable: "Onderhoud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedewerkerOnderhoud1",
                columns: table => new
                {
                    DoetId = table.Column<int>(type: "int", nullable: false),
                    MedewerkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedewerkerOnderhoud1", x => new { x.DoetId, x.MedewerkerId });
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud1_Medewerker_MedewerkerId",
                        column: x => x.MedewerkerId,
                        principalTable: "Medewerker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud1_Onderhoud_DoetId",
                        column: x => x.DoetId,
                        principalTable: "Onderhoud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gast_BegeleiderId",
                table: "Gast",
                column: "BegeleiderId");

            migrationBuilder.CreateIndex(
                name: "IX_Gast_FavorietId",
                table: "Gast",
                column: "FavorietId");

            migrationBuilder.CreateIndex(
                name: "IX_Gast_GastInfoId",
                table: "Gast",
                column: "GastInfoId",
                unique: true,
                filter: "[GastInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedewerkerOnderhoud_CoordineertId",
                table: "MedewerkerOnderhoud",
                column: "CoordineertId");

            migrationBuilder.CreateIndex(
                name: "IX_MedewerkerOnderhoud1_MedewerkerId",
                table: "MedewerkerOnderhoud1",
                column: "MedewerkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Onderhoud_AttractieId",
                table: "Onderhoud",
                column: "AttractieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_AttractieId",
                table: "Reserveringen",
                column: "AttractieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_GastId",
                table: "Reserveringen",
                column: "GastId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedewerkerOnderhoud");

            migrationBuilder.DropTable(
                name: "MedewerkerOnderhoud1");

            migrationBuilder.DropTable(
                name: "Reserveringen");

            migrationBuilder.DropTable(
                name: "Medewerker");

            migrationBuilder.DropTable(
                name: "Onderhoud");

            migrationBuilder.DropTable(
                name: "Gast");

            migrationBuilder.DropTable(
                name: "Attracties");

            migrationBuilder.DropTable(
                name: "GastInfo");

            migrationBuilder.DropTable(
                name: "Gebruiker");
        }
    }
}
