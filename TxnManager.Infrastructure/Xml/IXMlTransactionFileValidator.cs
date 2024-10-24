using System.Collections.Generic;
using System.Xml.Linq;

namespace TxnManager.Infrastructure.Xml
{
    public interface IXMlTransactionFileValidator
    {
        bool IsValid(XDocument xDocument, out List<FileValidationResult> validationErrors);
    }
}