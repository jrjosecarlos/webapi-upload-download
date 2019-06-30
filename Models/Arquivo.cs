using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiUploadDownload.Models
{
    public abstract class Arquivo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Extensão")]
        public string Extensao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }
    }
}
