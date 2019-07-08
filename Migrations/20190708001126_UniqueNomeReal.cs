using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiUploadDownload.Migrations
{
    public partial class UniqueNomeReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NomeReal",
                table: "Arquivos",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arquivos_NomeReal",
                table: "Arquivos",
                column: "NomeReal",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Arquivos_NomeReal",
                table: "Arquivos");

            migrationBuilder.AlterColumn<string>(
                name: "NomeReal",
                table: "Arquivos",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);
        }
    }
}
