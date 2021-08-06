using System.Collections.Generic;
using System.Linq;
using Domain.Result;

namespace Domain
{
    public class Currency
    {
        public static readonly IReadOnlyCollection<string> ValidCurrencies = new [] { "USD", "EUR", "CHF" };

        private Currency(string code)
        {
            Code = code;
        }

        public static Result<Currency> Create(string code)
        {
            if (!ValidCurrencies.Contains(code)) return Errors.General.InvalidData($"Invalid currency code: {code}");
            
            return new Currency(code);
        }

        public string Code { get; }
    }
}