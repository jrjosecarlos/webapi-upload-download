using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Models
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
                .Property(b => b.DataCriacao)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ArquivoFS>()
                .HasBaseType<Arquivo>();
        }

        public DbSet<Arquivo> Arquivos { get; set; }
    }
}
