using System;
using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Data {
  public class KnownUserFromGoogleUserBuilder : IKnownUserFromGoogleUserBuilder {
    public KnownUserRecord BuildKnownUser(GoogleUserDataRecord googleUser) {
      if (googleUser == null) throw new ArgumentNullException(nameof(googleUser));

      if (string.IsNullOrEmpty(googleUser.Id)) throw new InvalidOperationException("The specified Google user does not have a correlation id, which is required.");

      return new KnownUserRecord {
        UniqueId = Guid.NewGuid(),
        ExternalCorrelationId = googleUser.Id,
        Type = 'G',
        FirstName = googleUser.Given_name,
        LastName = googleUser.Family_name,
      };
    }
  }
}