using System;
using Domain.Result;

namespace Domain
{
    public class Expense
    {
        protected Expense() {}

        public static Result<Expense> Create(User recipient, int amountInCents, Currency currency, Date date, string category)
        {
            if (amountInCents <= 0) return Errors.General.InvalidData("Expense amount must be positive");

            //This check is a bit unrealistic. just to illustrate how we check here all business rules during the creation of a new Expense.
            if (!recipient.IsActive) return Errors.Expenses.UserNotActive();

            return new Expense
            {
                Recipient = recipient,
                AmountInCents = amountInCents,
                Currency = currency,
                Category = category,
                Date = date
            };
        }

        public long Id { get; private set; }
        public virtual User Recipient { get; private set; }
        public Date Date { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public int AmountInCents { get; private set; }
        public Currency Currency { get; private set; }
        public string Category { get; private set; }
    }
}
