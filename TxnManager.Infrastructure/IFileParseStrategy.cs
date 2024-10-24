using System.Collections.Generic;
using System.IO;
using TxnManager.Domain.Model;

namespace TxnManager.Infrastructure
{
    public interface IFileParseStrategy
    {
        List<Transaction> Parse(Stream fileStream, FileExtension extension);
    }
}