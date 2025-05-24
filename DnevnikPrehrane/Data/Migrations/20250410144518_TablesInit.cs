using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnevnikPrehrane.Data.Migrations
{
    /// <inheritdoc />
    public partial class TablesInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Kalorije",
                table: "Namirnice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "KategorijaId",
                table: "Namirnice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Masti",
                table: "Namirnice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "Namirnice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ugljikohidrati",
                table: "Namirnice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Biljeske",
                columns: table => new
                {
                    BiljeskaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biljeske", x => x.BiljeskaId);
                    table.ForeignKey(
                        name: "FK_Biljeske_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    KategorijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.KategorijaId);
                    table.ForeignKey(
                        name: "FK_Kategorije_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Obroci",
                columns: table => new
                {
                    ObrokId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImeNamirnice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Količina = table.Column<double>(type: "float", nullable: false),
                    Kalorije = table.Column<double>(type: "float", nullable: false),
                    Protein = table.Column<double>(type: "float", nullable: false),
                    Ugljikohidrati = table.Column<double>(type: "float", nullable: false),
                    Masti = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obroci", x => x.ObrokId);
                    table.ForeignKey(
                        name: "FK_Obroci_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZapisiMase",
                columns: table => new
                {
                    ZapisMaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Masa = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZapisiMase", x => x.ZapisMaseId);
                    table.ForeignKey(
                        name: "FK_ZapisiMase_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Namirnice_KategorijaId",
                table: "Namirnice",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Biljeske_UserId",
                table: "Biljeske",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategorije_UserId",
                table: "Kategorije",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Obroci_UserId",
                table: "Obroci",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ZapisiMase_UserId1",
                table: "ZapisiMase",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Namirnice_Kategorije_KategorijaId",
                table: "Namirnice",
                column: "KategorijaId",
                principalTable: "Kategorije",
                principalColumn: "KategorijaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Namirnice_Kategorije_KategorijaId",
                table: "Namirnice");

            migrationBuilder.DropTable(
                name: "Biljeske");

            migrationBuilder.DropTable(
                name: "Kategorije");

            migrationBuilder.DropTable(
                name: "Obroci");

            migrationBuilder.DropTable(
                name: "ZapisiMase");

            migrationBuilder.DropIndex(
                name: "IX_Namirnice_KategorijaId",
                table: "Namirnice");

            migrationBuilder.DropColumn(
                name: "Kalorije",
                table: "Namirnice");

            migrationBuilder.DropColumn(
                name: "KategorijaId",
                table: "Namirnice");

            migrationBuilder.DropColumn(
                name: "Masti",
                table: "Namirnice");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "Namirnice");

            migrationBuilder.DropColumn(
                name: "Ugljikohidrati",
                table: "Namirnice");
        }
    }
}
