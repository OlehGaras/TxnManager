using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace TxnManager.Infrastructure.Xml
{
    public class XmlTransactionFileValidator: IXMlTransactionFileValidator
    {
        private readonly int _idMaxLength = 50;
        private readonly List<string> _iso4217CurrencySymbols;

        public XmlTransactionFileValidator()
        {
            _iso4217CurrencySymbols = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
                .Distinct()
                .ToList();
        }

        public bool IsValid(XDocument xDocument, out List<FileValidationResult> validationErrors)
        {
            validationErrors = new List<FileValidationResult>();

            if (xDocument == null || !xDocument.Descendants(XName.Get("Transaction")).Any())
            {
                var error = new FileValidationResult();
                error.Messages.Add("Document has no transactions.");
                validationErrors.Add(error);
            }
            else
            {
                foreach (var transaction in xDocument.Descendants(XName.Get("Transaction")))
                {
                    var validationResult = ValidateSingleTransactionElement(transaction);
                    if (validationResult.HasErrors)
                    {
                        validationErrors.Add(validationResult);
                    }
                }
            }

            if (validationErrors.Any())
            {
                return false;
            }

            return true;
        }

        private FileValidationResult ValidateSingleTransactionElement(XElement transaction)
        {
            var validationResult = new FileValidationResult()
            {
                UnmappedRecord = transaction.ToString()
            };

            if (!HasAttribute(transaction, "id", out string idError))
            {
                validationResult.Messages.Add(idError);
            }
            else
            {
                var idLength = transaction.Attribute(XName.Get("id"))?.Value.Length;
                if (idLength > 50)
                {
                    validationResult.Messages.Add($"Id has more than {_idMaxLength} symbols. Length: {idLength}");
                }
            }

            if (!HasProperty(transaction, "TransactionDate", out string transDateError))
            {
                validationResult.Messages.Add(transDateError);
            }
            else
            {
                var transDateSrt = transaction.Element(XName.Get("TransactionDate"))?.Value;
                if(!DateTime.TryParse(transDateSrt, out DateTime transDate))
                {
                    validationResult.Messages.Add($"TransactionDate {transDateSrt} can not be parsed to DateTime.");
                }
            }

            if (!HasProperty(transaction, "PaymentDetails", out string paymentDetailsError))
            {
                validationResult.Messages.Add(paymentDetailsError);
            }
            else
            {
                var paymentDetailsElement = transaction.Element(XName.Get("PaymentDetails"));
                if (!HasProperty(paymentDetailsElement, "Amount", out string amountError))
                {
                    validationResult.Messages.Add(amountError);
                }

                if (!HasProperty(paymentDetailsElement, "CurrencyCode", out string currencyCodeError))
                {
                    validationResult.Messages.Add(currencyCodeError);
                }
                else
                {
                    var currencyCode = paymentDetailsElement.Element(XName.Get("CurrencyCode")).Value;
                    if (!_iso4217CurrencySymbols.Contains(currencyCode))
                    {
                        validationResult.Messages.Add($"{currencyCode} is not correct iso 4217 currency symbol.");
                    }
                }
            }

            if (!HasProperty(transaction, "Status", out string statusError))
            {
                validationResult.Messages.Add(paymentDetailsError);
            }
            else
            {
                var status = transaction.Element(XName.Get("Status")).Value;
                if (!Enum.TryParse(typeof(XmlTransactionStatus), status, true, out object result))
                {
                    validationResult.Messages.Add($"{status} can not be parsed to enum {nameof(XmlTransactionStatus)}");
                }
            }

            return validationResult;
        }

        private bool HasAttribute(XElement element, string attributeName, out string error)
        {
            var attribute = element.Attribute(XName.Get(attributeName));
            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                error = $"{attributeName} is not specified";
                return false;
            }

            error = null;
            return true;
        }

        private bool HasProperty(XElement element, string propertyName, out string error)
        {
            var attribute = element.Element(XName.Get(propertyName));
            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                error = $"{propertyName} is not specified";
                return false;
            }

            error = null;
            return true;
        }
    }
}