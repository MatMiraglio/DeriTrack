using System;
using Domain.Result;
using FluentAssertions;
using NUnit.Framework;

namespace DeriTrack.Tests
{
    class ResultTests
    {
        [Test]
        public void Result_fail_should_throw_when_error_is_null()
        {
            Action act = () => Result.Fail(null);
            Action act2 = () => Result.Fail<object>(null);

            act.Should().Throw<ArgumentNullException>();
            act2.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Result_ok_should_throw_when_value_is_null()
        {
            Action act = () => Result.Ok<object>(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Result_should_be_cast_from_error()
        {
            Result result = Errors.General.InvalidData("");

            result.IsFailure.Should().BeTrue();

            result.Error.Should().BeOfType<Error>();
        }

        [Test]
        public void Result_of_t_should_be_cast_from_error()
        {
            Result<MyTestClass> resultOfT = Errors.General.InvalidData("");

            resultOfT.IsFailure.Should().BeTrue();

            resultOfT.Error.Should().BeOfType<Error>();
        }

        [Test]
        public void Result_should_be_cast_from_value()
        {
            var myValue = new MyTestClass();

            Result<MyTestClass> result = myValue;

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<MyTestClass>();
            result.Value.Should().BeEquivalentTo(myValue);
        }

        [Test]
        public void Casting_failure_from_null_should_throw()
        {
            Error error = null;

            Action act = () => _ = (Result<MyTestClass>)error;

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Casting_success_from_null_should_throw()
        {
            MyTestClass value = null;

            Action act = () => _ = (Result<MyTestClass>)value;

            act.Should().Throw<ArgumentNullException>();
        }

        private class MyTestClass { }
    }
}
