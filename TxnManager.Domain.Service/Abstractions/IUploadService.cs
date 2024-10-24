using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TxnManager.Infrastructure;

namespace TxnManager.Domain.Service.Abstractions
{
    public interface IUploadService
    {
        Task UploadTransactionsFileAsync(Stream fileStream, FileExtension fileExtension,
            CancellationToken cancellationToken);
    }
}