using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Models
{
    public class ArquivoFS : Arquivo
    {
        [Required]
        [StringLength(100)]
        public string Caminho { get; set; }
    }
}
