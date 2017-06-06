using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Domain.DTO;
using FluentValidation;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  public class DisconnectGooglePlusUserRequestValidator : NullAllowableValidator<DisconnectGooglePlusUserRequest> {
    public DisconnectGooglePlusUserRequestValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.")
        .Must(ctx => ctx?.GetAuthenticatedUser()?.Type == UserType.GooglePlus)
        .WithMessage("Only an external user can be disconnected from this app.");
    }
  }
}