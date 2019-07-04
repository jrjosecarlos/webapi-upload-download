using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiUploadDownload.Models
{
    public class Arquivo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(80)]
        public string Nome { get; set; }

        [StringLength(200)]
        public string Caminho { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }

        public ArquivoDB ArquivoDB { get; set; }
    }
}
