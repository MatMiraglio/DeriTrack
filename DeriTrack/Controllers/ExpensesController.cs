using System.ComponentModel.DataAnnotations;
using System.Linq;
using DeriTrack.Domain;
using DeriTrack.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeriTrack.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    public class ExpensesController : ControllerBase
    {

        private readonly ILogger<ExpensesController> _logger;
        private readonly Context _context;

        public ExpensesController(ILogger<ExpensesController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [Route("all-expenses")]
        public IActionResult Get()
        {
            var expenses = _context.Expenses.Select(x => new
            {
                x.Id,
                RecipientEmail = x.Recipient.Email.ToString(),
                Amount = x.AmountInCents,
                Currency = x.Currency.Code,
                Date = (string) x.Date,
                x.Category
            }).ToList();

            return Ok(expenses);
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [Route("search-expenses")]
        public IActionResult Get(int page, int pageSize, long? userId)
        {
            var expenses = _context.Expenses.AsQueryable();

            if (userId.HasValue)
            {
                expenses = expenses.Where(x => x.Recipient.Id == userId);
            }

            var response  = expenses.Select(x => new
                {
                    x.Id,
                    RecipientEmail = x.Recipient.Email,
                    Amount = x.AmountInCents,
                    Currency = x.Currency.Code,
                    x.Category
                })
                .Paginated(page, pageSize);

            return Ok(response);
        }

        public class CreateExpenseRequest
        {
            [Required, EmailAddress, UserEmail]
            public string RecipientEmail { get; set; }

            [Range(1, int.MaxValue)]
            public int AmountInCents { get; set; }

            [Required, Currency]
            public string CurrencyCode { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            public string Date { get; set; }
        }
        
        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [Route("create-expense")]
        public IActionResult CreateExpense(CreateExpenseRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = _context.User.Single( x => x.Email == request.RecipientEmail);

            var expense = Expense.Create(
                                                        user, 
                                                        request.AmountInCents, 
                                                        Currency.Create(request.CurrencyCode),
                                                        Date.Create(request.Date),
                                                        request.Category
                                                    );

            //This will never be reached right now, just as illustration of how the right error would be return in the response
            if (expense.IsFailure) return BadRequest(expense.Error);

            _context.Expenses.Add(expense);

            _context.SaveChanges();

            return Ok();
        }
    }
}
