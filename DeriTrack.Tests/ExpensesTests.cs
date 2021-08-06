using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Result;
using NUnit.Framework;

namespace DeriTrack.Tests
{
    class ExpensesTests
    {

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void The_amount_of_an_expense_is_always_expressed_in_positive_values(int invalidAmount)
        {
            var user = new User
            {
                IsActive = true
            };

            var expense = Expense.Create(
                user,
                invalidAmount,
                Currency.Create("USD"),
                new Date(DateTime.Today),
                "test"
            );

            Assert.True(expense.IsFailure);
            Assert.AreEqual(expense.Error, Errors.General.InvalidData(""));
            Assert.True(expense.Error.Message.Contains("positive"));
        }

        [Test]
        public void Expense_cannot_be_created_for_inactive_user()
        {
            var inactiveUser = new User
            {
                IsActive = false
            };

            var expense = Expense.Create(
                inactiveUser, 
                1,
                Currency.Create("USD"),
                new Date(DateTime.Today),
                "test"
                );

            Assert.True(expense.IsFailure);
            Assert.AreEqual(expense.Error, Errors.Expenses.UserNotActive());
        }
    }
}
