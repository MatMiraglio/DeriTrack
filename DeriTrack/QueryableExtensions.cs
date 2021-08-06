using System.Linq;

namespace DeriTrack
{
    internal static class QueryableExtensions
    {
        public static IQueryable<T> WithPagination<T>(this IQueryable<T> entities, out int count, int page, int? pageSize)
        {
            count = entities.Count();

            // if no page size set - return all
            if (!pageSize.HasValue) return entities;

            return entities
                .Skip((page - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        public static Paging<T> Paginated<T>(this IQueryable<T> entities, int page, int? pageSize)
        {
            var elements = entities
                .WithPagination(out var count, page, pageSize)
                .ToList();

            return new Paging<T>(count, page, pageSize, elements);
        }
    }
}