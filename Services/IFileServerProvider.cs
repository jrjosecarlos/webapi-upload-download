using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUploadDownload.Services
{
    /// <summary>
    /// Interface responsável por prover serviços de upload/download, baseado em <see cref="Stream"/>.
    /// </summary>
    public interface IFileServerProvider
    {
        /// <summary>
        /// Realiza assincronamente o upload de um arquivo, a partir do conteúdo de uma <see cref="Stream"/>.
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo que será criado</param>
        /// <param name="stream">Stream com o conteúdo do arquivo para upload. Esta Stream já deve estar aberta.</param>
        /// <returns>Uma <see cref="Task"/> que representa a tarefa do upload a ser feito.</returns>
        Task UploadFromStreamAsync(string nomeArquivo, Stream stream);

        /// <summary>
        /// Realiza assincronamente o download de um arquivo, que será criado com o nome solicitado.
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo que será baixado.</param>
        /// <returns>Uma <see cref="Task{Stream}"/> que representa a tarefa do download que será feito.</returns>
        Task<Stream> GetDownloadStreamAsync(string nomeArquivo);
    }
}
