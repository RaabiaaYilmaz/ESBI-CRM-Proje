using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class FiltreliBenzersizIndexler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Malzemeler_MalzemeKodu",
                table: "Malzemeler");

            migrationBuilder.DropIndex(
                name: "IX_Cariler_CariKodu",
                table: "Cariler");

            migrationBuilder.CreateIndex(
                name: "IX_Malzemeler_MalzemeKodu",
                table: "Malzemeler",
                column: "MalzemeKodu",
                unique: true,
                filter: "[AktifMi] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Cariler_CariKodu",
                table: "Cariler",
                column: "CariKodu",
                unique: true,
                filter: "[AktifMi] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Malzemeler_MalzemeKodu",
                table: "Malzemeler");

            migrationBuilder.DropIndex(
                name: "IX_Cariler_CariKodu",
                table: "Cariler");

            migrationBuilder.CreateIndex(
                name: "IX_Malzemeler_MalzemeKodu",
                table: "Malzemeler",
                column: "MalzemeKodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cariler_CariKodu",
                table: "Cariler",
                column: "CariKodu",
                unique: true);
        }
    }
}
