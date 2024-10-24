using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using TxnManager.Domain.Model;

namespace TxnManager.Infrastructure.Xml
{
    public class XmlFileParser: IFileParser
    {
        private readonly IXMlTransactionFileValidator _transactionFileValidator;
        private readonly ILogger<XmlFileParser> _logger;

        public XmlFileParser(IXMlTransactionFileValidator transactionFileValidator,
            ILogger<XmlFileParser> logger)
        {
            _transactionFileValidator = transactionFileValidator;
            _logger = logger;
        }

        public List<Transaction> Parse(Stream fileStream)
        {
            var xDocument = XDocument.Load(fileStream);

            // some fluent validation could be done after deserializing 
            // but I wanted to play with xml file
            if (!_transactionFileValidator.IsValid(xDocument, out List<FileValidationResult> validationErrors))
            {
                validationErrors.ForEach(error => _logger.LogError(error.ToString()));
                throw new FileParseException("Errors found inside xml file", validationErrors);
            }

            XmlSerializer xml = new XmlSerializer(typeof(XmlTransactionsRecord));
            var result = xml.Deserialize(xDocument.CreateReader()) as XmlTransactionsRecord;

            var mappedRecords = result?.Transactions?.ToList();

            var transactions = mappedRecords?
                .Select(record => new Transaction(record.TransactionId, record.PaymentDetails.Amount, 
                    record.PaymentDetails.CurrencyCode, record.TransactionDate, (TransactionStatus)(int) record.Status))
                .ToList();

            return transactions;
        }

        public bool IsApplicable(FileExtension fileExtension)
        {
            return fileExtension == FileExtension.Xml;
        }
    }
}