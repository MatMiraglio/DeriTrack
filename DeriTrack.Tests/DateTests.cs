using NUnit.Framework;

namespace DeriTrack.Tests
{
    public class DateTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("2020-05-04", "2021-05-04")] // different year
        [TestCase("2021-02-04", "2021-05-04")] // different month
        [TestCase("2021-05-11", "2021-05-04")] // different day
        public void Dates_with_different_day_month_or_year_are_unequal(string firstDateString, string secondDateString)
        {
            var firstDate = new Date(firstDateString);
            var secondDate = new Date(secondDateString);

            Assert.False(firstDate == secondDate);
            Assert.False(firstDate.Equals(secondDate));
        }

        [Test]
        [TestCase("2021-05-04", "2021-05-04")] // same date
        [TestCase("2021-05-04T11:00:00", "2021-05-04T13:00:00")] // same date different hour
        [TestCase("2021-05-04T13:00:00", "2021-05-04T13:47:00")] // same date different minute
        [TestCase("2021-05-04T13:00:00", "2021-05-04T13:00:15")] // same date different second
        public void Dates_with_same_day_month_and_year_are_equal(string firstDateString, string secondDateString)
        {
            var firstDate = new Date(firstDateString);
            var secondDate = new Date(secondDateString);

            Assert.True(firstDate == secondDate);
            Assert.True(firstDate.Equals(secondDate));
        }
    }
}