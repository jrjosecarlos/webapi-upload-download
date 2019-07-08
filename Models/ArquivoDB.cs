using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Models
{
    public class ArquivoDB
    {
        [Key]
        public int ArquivoID { get; set; }

        [JsonIgnore]
        public byte[] Conteudo { get; set; }

        public Arquivo Arquivo { get; set; }
    }
}
