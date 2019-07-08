using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUploadDownload.Models;

namespace WebApiUploadDownload.Data
{
    public class WebApiUploadDownloadContext : DbContext
    {
        public WebApiUploadDownloadContext(DbContextOptions<WebApiUploadDownloadContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arquivo>()
                .Property(a => a.DataCriacao)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Arquivo>()
                .HasIndex(a => a.NomeReal)
                .IsUnique();
        }

        public DbSet<Arquivo> Arquivos { get; set; }
    }
}
