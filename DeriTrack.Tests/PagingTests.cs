using System.Collections.Generic;
using NUnit.Framework;

namespace DeriTrack.Tests
{
    class PagingTests
    {
       
        [Test]
        [TestCase(40, 1, 10, null, 2)]
        [TestCase(30, 3, 10, 2, null)]
        [TestCase(50, 2, 20, 1, 3)]
        [TestCase(10, 1, 10, null, null)]
        [TestCase(5, 1, 10, null, null)]
        public void Pagination_should_be_created(int count, int currentPage, int pageSize, int? expectedPrev, int? expectedNext)
        {
            var collection = new List<object>();
            var page = new Paging<object>(count, currentPage, pageSize, collection);

            Assert.AreEqual(page.Next, expectedNext);
            Assert.AreEqual(page.Prev, expectedPrev);
        }
        
    }
}
