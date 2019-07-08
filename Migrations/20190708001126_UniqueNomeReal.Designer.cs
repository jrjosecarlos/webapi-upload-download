﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiUploadDownload.Data;

namespace WebApiUploadDownload.Migrations
{
    [DbContext(typeof(WebApiUploadDownloadContext))]
    [Migration("20190708001126_UniqueNomeReal")]
    partial class UniqueNomeReal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiUploadDownload.Models.Arquivo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCriacao")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("NomeReal")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("NomeReal")
                        .IsUnique();

                    b.ToTable("Arquivos");
                });

            modelBuilder.Entity("WebApiUploadDownload.Models.ArquivoDB", b =>
                {
                    b.Property<int>("ArquivoID");

                    b.Property<byte[]>("Conteudo");

                    b.HasKey("ArquivoID");

                    b.ToTable("ArquivoDB");
                });

            modelBuilder.Entity("WebApiUploadDownload.Models.ArquivoDB", b =>
                {
                    b.HasOne("WebApiUploadDownload.Models.Arquivo", "Arquivo")
                        .WithOne("ArquivoDB")
                        .HasForeignKey("WebApiUploadDownload.Models.ArquivoDB", "ArquivoID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
