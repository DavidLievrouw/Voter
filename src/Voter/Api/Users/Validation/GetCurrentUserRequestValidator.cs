using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Common;
using FluentValidation;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  public class GetCurrentUserRequestValidator : NullAllowableValidator<GetCurrentUserRequest> {
    public GetCurrentUserRequestValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}