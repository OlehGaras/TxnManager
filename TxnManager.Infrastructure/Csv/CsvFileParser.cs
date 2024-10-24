using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using TinyCsvParser;
using TxnManager.Domain.Model;

namespace TxnManager.Infrastructure.Csv
{
    public class CsvFileParser : IFileParser
    {
        private readonly ILogger<CsvFileParser> _logger;

        public CsvFileParser(ILogger<CsvFileParser> logger)
        {
            _logger = logger;
        }

        public List<Transaction> Parse(Stream fileStream)
        {
            var csvParserOptions = new CsvParserOptions(false, ',');
            var csvMapper = new CsvTransactionMapping();
            var csvParser = new CsvParser<CsvTransactionRecord>(csvParserOptions, csvMapper);

            var results = csvParser
                .ReadFromStream(fileStream, Encoding.UTF8)
                .ToList();

            var validationErrorResults = results
                    .Where(result => !result.IsValid)
                    .Select(result =>
                    {
                        var validationResult = new FileValidationResult()
                        {
                            UnmappedRecord = result.Error.UnmappedRow
                        };
                        validationResult.Messages.Add(result.Error.Value);

                        return validationResult;
                    }).ToList();

            if (validationErrorResults.Any())
            {
                validationErrorResults.ForEach(error => _logger.LogError(error.ToString()));
                throw new FileParseException("Errors found inside csv file", validationErrorResults);
            }

            var mappedRecords = results
                .Where(result => result.IsValid)
                .Select(result => result.Result)
                .ToList();

            var transactions = mappedRecords
                .Select(record => new Transaction(record.TransactionId, record.Amount, record.CurrencyCode, 
                    record.TransactionDate, (TransactionStatus)(int)record.Status))
                .ToList();

            return transactions;
        }

        public bool IsApplicable(FileExtension fileExtension)
        {
            return fileExtension == FileExtension.Csv;
        }
    }
}