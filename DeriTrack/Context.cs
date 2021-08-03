using System.Collections.Generic;
using System.Linq;
using DeriTrack.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeriTrack
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Expense

            modelBuilder.Entity<Expense>()
                .Property(b => b.Currency)
                .HasConversion(x => x.Code, x => Currency.Create(x));

            modelBuilder.Entity<Expense>()
                .Property(b => b.Date)
                .HasConversion(x => x.ToString(), x => Date.Create(x));

            modelBuilder.Entity<Expense>()
                .HasIndex(b => b.CreatedAt);

            modelBuilder.Entity<Expense>()
                .Property(b => b.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("getdate()");

            #endregion

            #region User

            modelBuilder.Entity<User>()
                .Property(b => b.Email)
                .HasConversion(x => (string) x, x => Email.Create(x));

            #endregion

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                Email = Email.Create("matias.miraglio@vontobel.ch"),
                IsActive = true
            });
        }

        public DbSet<User> User { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        //public DbSet<Currency> Currencies { get; set; }
    }

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