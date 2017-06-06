using FluentValidation;
using FluentValidation.Results;

namespace DavidLievrouw.Voter.Common {
  public abstract class NullAllowableValidator<T> : AbstractValidator<T> {
    protected bool IsNullAllowed { get; set; }

    public override ValidationResult Validate(ValidationContext<T> context) {
      if (IsNullAllowed) {
        return context.InstanceToValidate == null
          ? new ValidationResult()
          : base.Validate(context);
      } else {
        return context.InstanceToValidate == null
          ? new ValidationResult(new[] {new ValidationFailure(typeof(T).Name, typeof(T).Name + " instance cannot be null.")})
          : base.Validate(context);
      }
    }
  }
}