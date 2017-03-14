using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Common;
using FluentValidation;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  public class LoginLocalUserRequestValidator : NullAllowableValidator<LoginLocalUserRequest> {
    public LoginLocalUserRequestValidator() {
      RuleFor(req => req.Login)
        .NotNull()
        .WithMessage("A valid login should be specified.");
      RuleFor(req => req.Password)
        .NotNull()
        .WithMessage("A valid password should be specified.");
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}