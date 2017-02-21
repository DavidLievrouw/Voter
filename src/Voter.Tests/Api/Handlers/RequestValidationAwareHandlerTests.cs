﻿using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Handlers {
  [TestFixture]
  public class RequestValidationAwareHandlerTests {
    IHandler<ThingToValidate, object> _decoratedHandler;
    IValidator<ThingToValidate> _validator;
    RequestValidationAwareHandler<ThingToValidate, object> _sut;
    ThingToValidate _thingToValidate;

    [SetUp]
    public void SetUp() {
      _decoratedHandler = A.Fake<IHandler<ThingToValidate, object>>();
      _validator = A.Fake<IValidator<ThingToValidate>>();
      _sut = new RequestValidationAwareHandler<ThingToValidate, object>(_validator, _decoratedHandler);
      _thingToValidate = new ThingToValidate();
    }

    [Test]
    public void ConstructorTests() {
      _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
    }
    
    [Test]
    public void WhenValidationFails_ThrowsExceptionWithValidationResult() {
      A.CallTo(() => _validator.Validate(_thingToValidate))
        .Returns(Models.ValidationResultFailure);

      Func<Task> act = () => _sut.Handle(_thingToValidate);
      act.ShouldThrow<RequestValidationException>().Where(ex => ex.ValidationResult == Models.ValidationResultFailure);
    }

    [Test]
    public async Task WhenValidationSucceeds_DelegatesControlToInnerHandler() {
      A.CallTo(() => _validator.Validate(_thingToValidate))
        .Returns(Models.ValidationResultSuccess);

      var expectedResult = new object();
      A.CallTo(() => _decoratedHandler.Handle(_thingToValidate))
        .Returns(expectedResult);

      var actual = await _sut.Handle(_thingToValidate);

      Assert.That(actual, Is.EqualTo(expectedResult));
      A.CallTo(() => _validator.Validate(_thingToValidate))
        .MustHaveHappened();
      A.CallTo(() => _decoratedHandler.Handle(_thingToValidate))
        .MustHaveHappened();
    }

    [Test]
    public void WhenValidationThrows_ThrowsWrappedException() {
      var validationError = new Exception("Something went wrong during validation.");
      A.CallTo(() => _validator.Validate(_thingToValidate))
        .Throws(validationError);

      Func<Task> act = () => _sut.Handle(_thingToValidate);
      act.ShouldThrow<RequestValidationException>().Where(ex => ex.ValidationResult == null && ex.InnerException.Equals(validationError));
    }

    public class ThingToValidate { }
  }
}