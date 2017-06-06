using System;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Domain.DTO.Adapters {
  public class UserAdapter : IAdapter<KnownUserRecord, User> {
    public User Adapt(KnownUserRecord input) {
      if (input == null) return null;

      return new User {
        UniqueId = input.UniqueId,
        FirstName = input.FirstName,
        LastNamePrefix = input.LastNamePrefix,
        LastName = input.LastName,
        Login = new Login {Value = input.Login},
        Password = new Password {
          Value = input.Password,
          IsEncrypted = true,
          Salt = input.Salt
        },
        Type = Enum.IsDefined(typeof(UserType), (int) input.Type)
          ? (UserType) input.Type
          : throw new NotSupportedException($"The specified user type ({input.Type}) is not supported."),
        ExternalCorrelationId = new ExternalCorrelationId {Value = input.ExternalCorrelationId}
      };
    }
  }
}