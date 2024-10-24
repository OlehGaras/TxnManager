using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TxnManager.Web.Attributes
{
    public class AllowedFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public AllowedFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxSize)
                {
                    return new ValidationResult($"{file.Length} bytes is greater than allowed size: {_maxSize} bytes.");
                }
            }

            return ValidationResult.Success;
        }
    }
}