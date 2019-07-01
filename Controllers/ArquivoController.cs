using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiUploadDownload.Models;
using WebApiUploadDownload.Models.ViewModels;

namespace WebApiUploadDownload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArquivoController : ControllerBase
    {
        private readonly WebApiUploadDownloadContext _context;

        public ArquivoController(WebApiUploadDownloadContext context)
        {
            _context = context;
        }

        // GET: api/Arquivo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArquivoViewModel>>> GetArquivos()
        {
            //TODO: Código de inicialização para testes, será removido futuramente
            if (_context.Arquivos.Count() == 0)
            {

                _context.Arquivos.Add(new Arquivo
                {
                    Nome = "Arquivo bin",
                    ArquivoDB = new ArquivoDB
                    {
                        Conteudo = new byte[] { 0x30 }
                    }
                });
                _context.Add(new Arquivo
                {
                    Nome = "Arquivo teste",
                    Caminho = "pasta/teste"
                });
                _context.SaveChanges();
            }

            var arquivos = _context.Arquivos
                //.Include(a => a.ArquivoDB)
                .Select(a => new ArquivoViewModel
                {
                    ID = a.ID,
                    Nome = a.Nome,
                    Caminho = a.Caminho,
                    DataCriacao = a.DataCriacao,
                    HasArquivoDB = a.ArquivoDB != null
                })
                .AsNoTracking();

            return await arquivos.ToListAsync();
        }

    }
}
