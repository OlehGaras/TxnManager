using System;
using System.Collections.Generic;

namespace TxnManager.Infrastructure
{
    public class FileParseException : Exception
    {
        public List<FileValidationResult> ErrorResults { get; }
        public FileParseException(string message, List<FileValidationResult> errorResults) : base(message)
        {
            this.ErrorResults = errorResults;
        }
    }
}