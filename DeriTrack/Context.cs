using System.Collections.Generic;
using Domain;
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
}