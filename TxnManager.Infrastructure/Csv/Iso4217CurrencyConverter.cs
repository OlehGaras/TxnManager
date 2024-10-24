using System.Globalization;
using System.Linq;
using TinyCsvParser.TypeConverter;

namespace TxnManager.Infrastructure.Csv
{
    public class Iso4217CurrencyConverter: NonNullableConverter<string>
    {
        protected override bool InternalConvert(string value, out string result)
        {
            var currencySymbols = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
                .Distinct();

            if (currencySymbols.Contains(value))
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }
    }
}