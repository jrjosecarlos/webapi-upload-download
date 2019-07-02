using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<ArquivoBaseViewModel>>> GetArquivos()
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
                .Select(a => new ArquivoBaseViewModel
                {
                    ID = a.ID,
                    Nome = a.Nome,
                    Caminho = a.Caminho,
                    DataCriacao = a.DataCriacao,
                    IsArquivoDB = a.ArquivoDB != null
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
        public async Task<ActionResult<Arquivo>> PostTodoItem([FromForm] ArquivoUploadViewModel arquivoUploadVM)
        {
            // TODO: Adicionar verificação da desserialização, para retornar um código 400
            var jsonPayload = arquivoUploadVM.Payload;
            var novoArquivo = JsonConvert.DeserializeObject<Arquivo>(await new StringReader(jsonPayload).ReadToEndAsync());

            if (!TryValidateModel(novoArquivo))
            {
                return BadRequest(ModelState);
            }

            IFormFile arquivo = arquivoUploadVM.Arquivo;
            byte[] myFileContent;

            // TODO: ler corretamente
            using (var memoryStream = new MemoryStream())
            {
                await arquivo.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                myFileContent = new byte[memoryStream.Length];
                await memoryStream.ReadAsync(myFileContent, 0, myFileContent.Length);
            }


            _context.Arquivos.Add(novoArquivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArquivo), new { id = novoArquivo.ID }, novoArquivo);
        }

    }
}
