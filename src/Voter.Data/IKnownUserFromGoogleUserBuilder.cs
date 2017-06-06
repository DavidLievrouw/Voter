using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Data {
  public interface IKnownUserFromGoogleUserBuilder {
    KnownUserRecord BuildKnownUser(GoogleUserDataRecord googleUser);
  }
}