using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeriTrack
{
    public class Expense
    {
        protected Expense() {}

        public static Result<Expense> Create(User recipient, int amountInCents, Currency currency, string category)
        {
            if (amountInCents < 0) return Errors.General.InvalidData("Expense amount must be positive");

            //This check is a bit unrealistic. just to illustrate how we check here all business rules during the creation of a new Expense.
            if (!recipient.LockoutEnabled) return Errors.General.InvalidData("Expense amount must be positive");

            return new Expense
            {
                Recipient = recipient,
                AmountInCents = amountInCents,
                Currency = currency,
                Category = category
            };
        }

        public long Id { get; private set; }
        public virtual User Recipient { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public int AmountInCents { get; private set; }
        public Currency Currency { get; private set; }
        public string Category { get; private set; }
    }

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

    public class User : IdentityUser<long>
    {
    }

    public class Email : ValueObject
    {
        private readonly string _value;

        public static Result<Email> Create(string email)
        {
            var result = new Regex("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$\r\n");

            if (!result.IsMatch(email)) return Errors.General.InvalidData("invalid email format");

            return new Email(email);
        }

        private Email(string value)
        {
            _value = value;
        }

        public string Domain => _value.Split('@')[1];

        public static implicit operator string(Email email)
        {
            return email._value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }
    }
}
