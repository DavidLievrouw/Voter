using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Models;

namespace DavidLievrouw.Voter.Api.Users.Models.Adapters {
  public class UserAdapter : IAdapter<Domain.DTO.User, User> {
    public User Adapt(Domain.DTO.User input) {
      if (input == null) return null;
      return new User {
        FirstName = input.FirstName,
        LastNamePrefix = input.LastNamePrefix,
        UniqueId = input.UniqueId,
        LastName = input.LastName
      };
    }
  }
}