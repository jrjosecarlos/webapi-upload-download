﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiUploadDownload.Data;
using WebApiUploadDownload.Models;
using WebApiUploadDownload.Models.ViewModels;
using WebApiUploadDownload.Services;

namespace WebApiUploadDownload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArquivoController : ControllerBase
    {
        private readonly WebApiUploadDownloadContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IFileServerProvider _fileServerProvider;

        public ArquivoController(WebApiUploadDownloadContext context, IHostingEnvironment env,
            IFileServerProvider fileServerProvider)
        {
            _context = context;
            _env = env;
            _fileServerProvider = fileServerProvider;
        }

        public async Task<bool> VerificarArquivoUnico(string nomeReal)
        {
            var arquivoExistente = await _context.Arquivos
                .Where(a => a.NomeReal == nomeReal)
                .FirstOrDefaultAsync();

            return arquivoExistente != null;
        }

        // GET: api/Arquivo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArquivoBaseViewModel>>> GetArquivos()
        {
            var arquivos = _context.Arquivos
                .Select(a => new ArquivoBaseViewModel
                {
                    ID = a.ID,
                    Nome = a.Nome,
                    DataCriacao = a.DataCriacao,
                    IsArquivoDB = a.ArquivoDB != null
                })
                .AsNoTracking();

            return await arquivos.ToListAsync();
        }

        private string SanitizarNomeArquivo(string nomeArquivo)
        {
            var caracteresInvalidos = Path.GetInvalidFileNameChars();
            return String.Join("_", nomeArquivo.Split(caracteresInvalidos, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }

        // GET: api/Arquivo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArquivo(int id)
        {
            var arquivo = await _context.Arquivos
                .Include(a => a.ArquivoDB)
                .Where(a => a.ID == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (arquivo == null)
            {
                return NotFound();
            }

            Stream stream;
            try
            {
                if (arquivo.ArquivoDB == null)
                {
                    stream = await _fileServerProvider.GetDownloadStreamAsync(arquivo.NomeReal);

                }
                else
                {
                    stream = new MemoryStream(arquivo.ArquivoDB.Conteudo, false);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro ao recuperar arquivo: {e}");
            }

            return File(stream, "application/octet-stream", arquivo.NomeReal);
        }

        // POST: api/Arquivo
        [HttpPost]
        public async Task<ActionResult<Arquivo>> PostTodoItem([FromForm] ArquivoUploadViewModel arquivoUploadVM)
        {
            var jsonPayload = arquivoUploadVM.Payload;

            ArquivoBaseViewModel arquivoBase;
            try
            {
                arquivoBase = JsonConvert.DeserializeObject<ArquivoBaseViewModel>(await new StringReader(jsonPayload).ReadToEndAsync());
            }
            catch (JsonReaderException)
            {
                return BadRequest();
            }

            IFormFile arquivoPayload = arquivoUploadVM.Arquivo;

            // Sanitizar o nome do arquivo


            Arquivo arquivo = arquivoBase.ToArquivo();

            arquivo.NomeReal = SanitizarNomeArquivo(arquivoPayload.FileName);

            if (!TryValidateModel(arquivo))
            {
                return ValidationProblem(ModelState);
            }

            // Verificar se já existe arquivo com o mesmo nome
            var arquivoExistente = await VerificarArquivoUnico(arquivo.NomeReal);

            if (arquivoExistente)
            {
                ModelState.AddModelError("Arquivo", $"Arquivo já existente: {arquivo.Nome}");
                return ValidationProblem(ModelState);
            }

            // Grava o arquivo em banco ou no File Server, dependendo do parâmetro
            try
            {
                if (arquivoBase.IsArquivoDB)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await arquivoPayload.CopyToAsync(memoryStream);
                        arquivo.ArquivoDB = new ArquivoDB
                        {
                            Conteudo = memoryStream.ToArray()
                        };
                    };
                }
                else
                {
                    using (var readStream = arquivoPayload.OpenReadStream())
                    {
                        await _fileServerProvider.UploadFromStreamAsync(arquivo.NomeReal, readStream);
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro ao enviar o arquivo: {e}");
            }

            // Grava a entrada no banco
            _context.Arquivos.Add(arquivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArquivo), new { id = arquivoBase.ID }, arquivoBase);
        }

    }
}
