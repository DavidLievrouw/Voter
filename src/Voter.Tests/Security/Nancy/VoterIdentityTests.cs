using DavidLievrouw.Voter.Domain.DTO;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Security.Nancy {
  [TestFixture]
  public class VoterIdentityTests {
    [Test]
    public void UserNameIsLoginValueOfSpecifiedUser() {
      var user = new User {
        Login = new Login {
          Value = "MyLogin"
        }
      };
      var actual = new VoterIdentity(user);
      Assert.That(actual.UserName, Is.EqualTo(user.Login.Value));
    }
  }
}