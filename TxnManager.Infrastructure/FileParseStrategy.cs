using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TxnManager.Domain.Model;

namespace TxnManager.Infrastructure
{
    public class FileParseStrategy: IFileParseStrategy
    {
        private readonly ILogger<FileParseStrategy> _logger;
        private readonly IEnumerable<IFileParser> _fileParsers;

        public FileParseStrategy(IServiceProvider serviceProvider,
            ILogger<FileParseStrategy> logger)
        {
            _logger = logger;
            _fileParsers = serviceProvider.GetServices<IFileParser>();
        }

        public List<Transaction> Parse(Stream fileStream, FileExtension extension)
        {
            try
            {
                var fileParser = _fileParsers.FirstOrDefault(parser => parser.IsApplicable(extension));

                if (fileParser == null)
                {
                    throw new FileParseException($"No file parser found to handle the file with extension: {extension}", null);
                }

                return fileParser.Parse(fileStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}