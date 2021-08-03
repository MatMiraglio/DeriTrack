using System.ComponentModel.DataAnnotations;
using System.Linq;
using DeriTrack.Domain;

namespace DeriTrack.Validation
{
    public sealed class UserEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success!;

            var context = (Context)validationContext
                .GetService(typeof(Context))!;

            if (!(value is string email)) return new ValidationResult("Value is not a string");

            var emailResult = Email.Create(email);

            if (!emailResult.IsSuccess) return new ValidationResult($"Email: '{email}' is not valid - {emailResult.Error.Message}");

            var emailExists = context.User.Any(x => x.Email == email);

            if (!emailExists) return new ValidationResult($"Email not registered");

            return ValidationResult.Success!;
        }
    }
}
