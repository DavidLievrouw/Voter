using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Common;
using FluentValidation;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  public class LogoutRequestValidator : NullAllowableValidator<LogoutRequest> {
    public LogoutRequestValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid Nancy context should be specified.");
    }
  }
}