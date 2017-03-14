using System;
using DavidLievrouw.Voter.Domain.DTO;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Security.Nancy {
  [TestFixture]
  public class VoterIdentityTests {
    [Test]
    public void UserNameIsUniqueIdentifierOfSpecifiedUser() {
      var user = new User {
        UniqueId = Guid.NewGuid(),
        Login = new Login {
          Value = "MyLogin"
        }
      };
      var actual = new VoterIdentity(user);
      Assert.That(actual.UserName, Is.EqualTo(user.UniqueId.ToString()));
    }
  }
}