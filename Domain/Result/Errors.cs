namespace Domain.Result
{
    public static class Errors
    {
        public static class General
        {
            public static Error InvalidData(string message) => new Error("invalid.data", message);

        }

        public static class Expenses
        {
            public static Error UserNotActive() => new Error("expense.user.not.active", "Expense cannot be submitted for user not yet active");
        }
    }
}