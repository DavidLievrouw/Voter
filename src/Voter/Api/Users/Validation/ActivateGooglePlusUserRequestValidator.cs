using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Common;
using FluentValidation;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  public class ActivateGooglePlusUserRequestValidator : NullAllowableValidator<ActivateGooglePlusUserRequest> {
    public ActivateGooglePlusUserRequestValidator() {
      RuleFor(req => req.IdToken)
        .NotNull()
        .NotEmpty()
        .WithMessage("A valid access token should be specified.");
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}