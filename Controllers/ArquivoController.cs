using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiUploadDownload.Models;

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

            //TODO: Código de inicialização para testes, será removido futuramente
            if (_context.Arquivos.Count() == 0)
            {
                _context.Arquivos.Add(new ArquivoFS { Nome = "Arquivo teste", Caminho = "pasta/teste" });
                _context.Arquivos.Add(new ArquivoDB { Nome = "Arquivo bin" });
                _context.SaveChanges();
            }

        }

        // GET: api/Arquivo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Arquivo>>> GetArquivos()
        {
            return await _context.Arquivos.ToListAsync();
        }

    }
}
