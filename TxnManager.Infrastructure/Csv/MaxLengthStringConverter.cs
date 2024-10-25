using TinyCsvParser.TypeConverter;

namespace TxnManager.Infrastructure.Csv
{
    public class MaxLengthStringConverter: NonNullableConverter<string>
    {
        private readonly int _maxLength;

        public MaxLengthStringConverter(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override bool InternalConvert(string value, out string result)
        {
            if (value.Length > _maxLength)
            {
                result = null;
                return false;
            }

            result = value;
            return true;
        }
    }
}