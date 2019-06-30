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
    }
}
