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

        //TODO: Marcado para remoção
        public string Caminho { get; set; }

        public DateTime DataCriacao { get; set; }

        public bool IsArquivoDB { get; set; }

        public Arquivo ToArquivo()
        {
            return new Arquivo
            {
                ID = this.ID,
                Nome = this.Nome,
                Caminho = this.Caminho,
                DataCriacao = this.DataCriacao
            };
        }

    }
}
