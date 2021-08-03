using System;
using System.Collections.Generic;

namespace DeriTrack
{
    public class Paging<T>
    {
        public Paging(int totalCount, int page, int? pageSize, IEnumerable<T> pageElements)
        {
            PageSize = pageSize ?? totalCount;
            Count = totalCount;
            int? numberOfPages = pageSize.HasValue ? (int?) Math.Ceiling((double)totalCount / pageSize.Value) : 1;
            int? next = page + 1;

            if (numberOfPages < next) next = null;
            Next = next;

            int? prev = page - 1;
            if (prev <= 0) prev = null;

            Prev = prev;
            Elements = pageElements;
        }

        public int Count { get; }
        public int? Next { get; }
        public int? PageSize { get; }
        public int? Prev { get; }
        public IEnumerable<T> Elements { get; }
    }
}