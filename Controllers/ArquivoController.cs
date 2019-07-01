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

        // GET: api/Arquivo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Arquivo>> GetArquivo(int id)
        {
            var arquivo = await _context.Arquivos.FindAsync(id);

            if (arquivo == null)
            {
                return NotFound();
            }

            return arquivo;
        }

        // POST: api/Arquivo
        [HttpPost]
        public async Task<ActionResult<Arquivo>> PostTodoItem(Arquivo arquivo)
        {
            _context.Arquivos.Add(arquivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArquivo), new { id = arquivo.ID }, arquivo);
        }

    }
}
