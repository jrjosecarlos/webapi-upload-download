using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiUploadDownload.Migrations
{
    public partial class CriacaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arquivos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 80, nullable: false),
                    NomeReal = table.Column<string>(maxLength: 200, nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquivos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArquivoDB",
                columns: table => new
                {
                    ArquivoID = table.Column<int>(nullable: false),
                    Conteudo = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivoDB", x => x.ArquivoID);
                    table.ForeignKey(
                        name: "FK_ArquivoDB_Arquivos_ArquivoID",
                        column: x => x.ArquivoID,
                        principalTable: "Arquivos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArquivoDB");

            migrationBuilder.DropTable(
                name: "Arquivos");
        }
    }
}
