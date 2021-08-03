using System.ComponentModel.DataAnnotations;
using System.Linq;
using DeriTrack.Domain;

namespace DeriTrack.Validation
{
    public sealed class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (!(value is string currencyCode)) return new ValidationResult("Value is not a string");

            var isValid = Currency.ValidCurrencies.Contains(currencyCode);

            if (!isValid) return new ValidationResult("invalid currency");

            return ValidationResult.Success;
        }
    }
}