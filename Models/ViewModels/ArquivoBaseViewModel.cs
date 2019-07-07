using Microsoft.AspNetCore.Http;
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

        public DateTime DataCriacao { get; set; }

        public bool IsArquivoDB { get; set; }

        public Arquivo ToArquivo()
        {
            return new Arquivo
            {
                ID = this.ID,
                Nome = this.Nome,
                DataCriacao = this.DataCriacao
            };
        }

    }
}
