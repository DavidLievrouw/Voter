using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Common;
using FluentValidation;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  public class LoginGooglePlusUserRequestValidator : NullAllowableValidator<LoginGooglePlusUserRequest> {
    public LoginGooglePlusUserRequestValidator() {
      RuleFor(req => req.Code)
        .NotNull()
        .WithMessage("A valid code should be specified.");
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}