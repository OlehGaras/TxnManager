using System;
using System.Xml.Serialization;

namespace TxnManager.Infrastructure.Xml
{
    [XmlRoot("Transactions")]
    public class XmlTransactionsRecord
    {
        [XmlElement("Transaction")]
        public XmlTransactionRecord[] Transactions { get; set; }
    }

    public class XmlTransactionRecord
    {
        [XmlAttribute("id")]
        public string TransactionId { get; set; }

        [XmlElement]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlElement]
        public DateTime TransactionDate { get; set; }

        [XmlElement]
        public XmlTransactionStatus Status { get; set; }
    }

    public class PaymentDetails
    {
        [XmlElement]
        public decimal Amount { get; set; }

        [XmlElement]
        public string CurrencyCode { get; set; }
    }

}