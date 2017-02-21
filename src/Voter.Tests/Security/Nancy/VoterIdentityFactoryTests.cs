using DavidLievrouw.Voter.Domain.DTO;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Security.Nancy {
  [TestFixture]
  public class VoterIdentityFactoryTests {
    VoterIdentityFactory _sut;

    [SetUp]
    public void SetUp() {
      _sut = new VoterIdentityFactory();
    }

    [Test]
    public void GivenNullUser_ReturnsNull() {
      var actual = _sut.Create(null);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenValidUser_CreatesIdentityByUser() {
      var user = new User();
      var actual = _sut.Create(user);

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.InstanceOf<VoterIdentity>());
      Assert.That(((VoterIdentity) actual).User, Is.EqualTo(user));
    }
  }
}