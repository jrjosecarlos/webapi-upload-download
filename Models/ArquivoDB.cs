using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Models
{
    public class ArquivoDB : Arquivo
    {
        public byte[] Conteudo { get; set; }
    }
}
