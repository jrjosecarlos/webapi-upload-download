using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
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
        private readonly IHostingEnvironment _env;
        private string BaseUploadFolder { get
            {
                return Path.Combine(_env.WebRootPath, "uploaded");
            } }

        public ArquivoController(WebApiUploadDownloadContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<IActionResult> GetArquivo(int id)
        {
            var arquivo = await _context.Arquivos.FindAsync(id);

            if (arquivo == null)
            {
                return NotFound();
            }

            var caminhoArquivo = Path.Combine(this.BaseUploadFolder, arquivo.Caminho);

            var fileInfo = new FileInfo(caminhoArquivo);

            if (!fileInfo.Exists)
            {
                return NotFound();
            }

            if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                return BadRequest();
            }

            return File(fileInfo.OpenRead(), "application/octet-stream", arquivo.Caminho);
        }

        // POST: api/Arquivo
        [HttpPost]
        public async Task<ActionResult<Arquivo>> PostTodoItem([FromForm] ArquivoUploadViewModel arquivoUploadVM)
        {
            var jsonPayload = arquivoUploadVM.Payload;

            Arquivo novoArquivo;
            try
            {
                novoArquivo = JsonConvert.DeserializeObject<Arquivo>(await new StringReader(jsonPayload).ReadToEndAsync());
            }
            catch (JsonReaderException)
            {
                return BadRequest();
            }

            if (!TryValidateModel(novoArquivo))
            {
                return BadRequest(ModelState);
            }

            IFormFile arquivo = arquivoUploadVM.Arquivo;
            //byte[] myFileContent;

            // TODO: ler corretamente
            /*using (var memoryStream = new MemoryStream())
            {
                await arquivo.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                myFileContent = new byte[memoryStream.Length];
                await memoryStream.ReadAsync(myFileContent, 0, myFileContent.Length);

            }*/

            // Separar toda a lógica de gravação de arquivo (e talvez de banco) para uma classe diferente, provavelmente um serviço
            // Garantir que a pasta de upload está criada
            Directory.CreateDirectory(this.BaseUploadFolder);

            // Sanitizar o nome do arquivo
            var caracteresInvalidos = Path.GetInvalidFileNameChars();
            var nomeArquivoSanitizado = String.Join("_", arquivo.FileName.Split(caracteresInvalidos, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

            using (var fileStream = new FileStream(Path.Combine(this.BaseUploadFolder, nomeArquivoSanitizado), FileMode.Create, FileAccess.Write))
            {
                await arquivo.CopyToAsync(fileStream);
            }

            novoArquivo.Caminho = nomeArquivoSanitizado;

            _context.Arquivos.Add(novoArquivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArquivo), new { id = novoArquivo.ID }, novoArquivo);
        }

    }
}
