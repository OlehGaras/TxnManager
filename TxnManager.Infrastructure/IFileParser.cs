using System.Collections.Generic;
using System.IO;
using TxnManager.Domain.Model;

namespace TxnManager.Infrastructure
{
    public interface IFileParser
    {
        List<Transaction> Parse(Stream fileStream);

        bool IsApplicable(FileExtension fileExtension);
    }
}
