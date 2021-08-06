using System.Collections.Generic;
using Domain.Result;

namespace Domain
{
    public class Email : ValueObject
    {
        private readonly string _value;

        public static Result<Email> Create(string email)
        {
            //Validation simplified.
            var isValid = email.Length > 3 && email.Contains("@") && email.Contains(".");

            if (!isValid) return Errors.General.InvalidData("invalid email format");

            return new Email(email);
        }

        private Email(string value)
        {
            _value = value;
        }

        public string Domain => _value.Split('@')[1];

        public static implicit operator string(Email email)
        {
            return email._value;
        }

        //TODO: replace workaround with JsonConverter
        public override string ToString()
        {
            return _value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }
    }
}