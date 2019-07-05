using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApiUploadDownload.Models.ViewModels
{
    public class ArquivoUploadViewModel : IValidatableObject
    {
        private readonly string[] ExtensoesInvalidas = { ".exe", ".bat" };

        private const long TamanhoMaximo = 10485760;

        [Required]
        public string Payload { get; set; }

        [Required]
        public IFormFile Arquivo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var nomeArquivo = Arquivo.FileName;

            if (ExtensoesInvalidas.Any(e =>
                    nomeArquivo.EndsWith(e, StringComparison.OrdinalIgnoreCase)
                ) )
            {
                yield return new ValidationResult($"Arquivo possui extensão inválida: {nomeArquivo}.", new string[] { "Arquivo" });
            }

            if (Arquivo.Length > TamanhoMaximo)
            {
                yield return new ValidationResult("O tamanho máximo do arquivo para upload é 10MB.", new string[] { "Arquivo" });
            }

        }
    }
}
