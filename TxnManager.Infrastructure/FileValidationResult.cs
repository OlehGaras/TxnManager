using System.Collections.Generic;
using System.Linq;

namespace TxnManager.Infrastructure
{
    public class FileValidationResult
    {
        public List<string> Messages { get; set; } = new List<string>();
        public string UnmappedRecord { get; set; }
        public bool HasErrors => Messages.Any();

        public override string ToString()
        {
            return $"Messages:{string.Join(',', Messages)}. Record: {UnmappedRecord}";
        }
    }
}