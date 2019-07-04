using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApiUploadDownload.Models.ViewModels
{
    public class ArquivoUploadViewModel
    {
        [Required]
        public string Payload { get; set; }

        [Required]
        public IFormFile Arquivo { get; set; }
    }
}
