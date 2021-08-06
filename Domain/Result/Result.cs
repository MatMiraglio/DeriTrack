using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Result
{
    public class Result
    {
        private readonly Error _error;
        public bool IsSuccess { get; }

        public Error Error
        {
            get
            {
                if (IsSuccess) throw new InvalidOperationException($"tried to access {nameof(Error)} of Success Result");

                return _error;
            }
        }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            _error = error;
        }

        public static Result Fail(Error error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            return new Result(isSuccess: false, error);
        }


        public static Result Ok()
        {
            return new Result(isSuccess: true, error: null);
        }

        public static Result<T> Ok<T>(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new Result<T>(value, isSuccess: true, error: null);
        }

        public static Result<T> Fail<T>(Error error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            return new Result<T>(value: default, isSuccess: false, error);
        }

        public static implicit operator Result(Error error)
        {
            return Fail(error);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (IsFailure) throw new InvalidOperationException("tried to access value of failed Result<T>");

                return _value;
            }
        }


        protected internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            return Ok(value);
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }

        public static implicit operator Result<T>(Error error)
        {
            return Fail<T>(error);
        }
    }

    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}
