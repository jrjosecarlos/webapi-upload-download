using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Models.ViewModels
{
    public class ArquivoBaseViewModel
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Caminho { get; set; }

        public DateTime DataCriacao { get; set; }

        public bool HasArquivoDB { get; set; }
    }
}
