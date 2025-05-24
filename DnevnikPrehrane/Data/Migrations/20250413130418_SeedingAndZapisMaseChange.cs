using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DnevnikPrehrane.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAndZapisMaseChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZapisiMase_AspNetUsers_UserId1",
                table: "ZapisiMase");

            migrationBuilder.DropIndex(
                name: "IX_ZapisiMase_UserId1",
                table: "ZapisiMase");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ZapisiMase");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ZapisiMase",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Kategorije",
                columns: new[] { "KategorijaId", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Voće", null },
                    { 2, "Povrće", null },
                    { 3, "Meso", null },
                    { 4, "Riba", null },
                    { 5, "Žitarice", null },
                    { 6, "Mliječni proizvodi", null },
                    { 7, "Ostalo", null }
                });

            migrationBuilder.InsertData(
                table: "Namirnice",
                columns: new[] { "NamirnicaId", "Kalorije", "KategorijaId", "Masti", "Name", "Protein", "Ugljikohidrati", "UserId" },
                values: new object[,]
                {
                    { 1, 52.0, 1, 0.20000000000000001, "Jabuka", 0.29999999999999999, 14.0, null },
                    { 2, 89.0, 1, 0.29999999999999999, "Banana", 1.1000000000000001, 23.0, null },
                    { 3, 47.0, 1, 0.10000000000000001, "Naranča", 0.90000000000000002, 12.0, null },
                    { 4, 41.0, 2, 0.20000000000000001, "Mrkva", 0.90000000000000002, 10.0, null },
                    { 5, 34.0, 2, 0.40000000000000002, "Brokula", 2.7999999999999998, 7.0, null },
                    { 6, 18.0, 2, 0.20000000000000001, "Rajčica", 0.90000000000000002, 3.8999999999999999, null },
                    { 7, 165.0, 3, 3.6000000000000001, "Pileća prsa", 31.0, 0.0, null },
                    { 8, 250.0, 3, 17.0, "Govedina", 26.0, 0.0, null },
                    { 9, 132.0, 4, 1.3, "Tuna", 28.0, 0.0, null },
                    { 10, 208.0, 4, 13.0, "Losos", 20.0, 0.0, null },
                    { 11, 265.0, 5, 3.2000000000000002, "Kruh", 9.0, 49.0, null },
                    { 12, 131.0, 5, 1.1000000000000001, "Tjestenina", 5.0, 25.0, null },
                    { 13, 42.0, 6, 1.0, "Mlijeko", 3.3999999999999999, 5.0, null },
                    { 14, 402.0, 6, 33.0, "Sir", 25.0, 1.3, null },
                    { 15, 59.0, 6, 0.40000000000000002, "Jogurt", 10.0, 3.6000000000000001, null },
                    { 16, 155.0, 7, 11.0, "Jaje", 13.0, 1.1000000000000001, null },
                    { 17, 884.0, 7, 100.0, "Maslinovo ulje", 0.0, 0.0, null },
                    { 18, 387.0, 7, 0.0, "Šećer", 0.0, 100.0, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZapisiMase_UserId",
                table: "ZapisiMase",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZapisiMase_AspNetUsers_UserId",
                table: "ZapisiMase",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZapisiMase_AspNetUsers_UserId",
                table: "ZapisiMase");

            migrationBuilder.DropIndex(
                name: "IX_ZapisiMase_UserId",
                table: "ZapisiMase");

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Namirnice",
                keyColumn: "NamirnicaId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Kategorije",
                keyColumn: "KategorijaId",
                keyValue: 7);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ZapisiMase",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ZapisiMase",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ZapisiMase_UserId1",
                table: "ZapisiMase",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ZapisiMase_AspNetUsers_UserId1",
                table: "ZapisiMase",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
