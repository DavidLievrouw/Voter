using System;
using System.Security;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Domain.DTO;
using FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Security.Nancy {
  [TestFixture]
  public class NancySecurityContextTests {
    INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    IVoterIdentityFactory _voterIdentityFactory;
    NancyContext _nancyContext;
    NancySecurityContext _sut;

    [SetUp]
    public void SetUp() {
      _nancySessionFromNancyContextResolver = _nancySessionFromNancyContextResolver.Fake();
      _voterIdentityFactory = _voterIdentityFactory.Fake();
      _nancyContext = new NancyContext();
      _sut = new NancySecurityContext(_nancyContext, _nancySessionFromNancyContextResolver, _voterIdentityFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(null, _nancySessionFromNancyContextResolver, _voterIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(_nancyContext, null, _voterIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(_nancyContext, _nancySessionFromNancyContextResolver, null));
    }

    public class SetAuthenticatedUser : NancySecurityContextTests {
      [Test]
      public void SetsUserInNancyContext() {
        var user = new User();
        var identity = new VoterIdentity(user);
        ConfigureVoterIdentityFactory_ToReturn(user, identity);

        _sut.SetAuthenticatedUser(user);
        var actual = _nancyContext.CurrentUser;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(identity));
      }

      [Test]
      public void SetsUserInSession() {
        var session = new FakeSession();
        ConfigureSessionFromContextResolver_ToReturn(session);
        var user = new User();

        _sut.SetAuthenticatedUser(user);
        var actual = session[Constants.SessionKeyForUser];

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(user));
      }

      [Test]
      public void GivenNullUser_SetsNullUserInNancyContext() {
        ConfigureVoterIdentityFactory_ToReturn(null, null);

        _sut.SetAuthenticatedUser(null);
        var actual = _nancyContext.CurrentUser;

        Assert.That(actual, Is.Null);
      }

      [Test]
      public void GivenNullUser_SetsNullUserInSession() {
        var session = new FakeSession();
        ConfigureSessionFromContextResolver_ToReturn(session);

        _sut.SetAuthenticatedUser(null);
        var actual = session[Constants.SessionKeyForUser];

        Assert.That(actual, Is.Null);
      }

      [Test]
      public void GivenNullUser_AbandonsSession() {
        var session = new FakeSession();
        ConfigureSessionFromContextResolver_ToReturn(session);

        _sut.SetAuthenticatedUser(null);

        Assert.That(session.IsAbandoned);
      }

      [Test]
      public void WhenNoSessionExists_Throws() {
        ConfigureSessionFromContextResolver_ToReturn(null);
        Assert.Throws<SecurityException>(() => _sut.SetAuthenticatedUser(new User()));
      }
    }

    public class GetAuthenticatedUser : NancySecurityContextTests {
      [Test]
      public void WhenNoIdentityIsSetInNancyContext_ReturnsNullUser() {
        _nancyContext.CurrentUser = null;
        var actual = _sut.GetAuthenticatedUser();
        Assert.That(actual, Is.Null);
      }

      [Test]
      public void WhenInvalidIdentityIsSetInNancyContext_ReturnsNullUser() {
        _nancyContext.CurrentUser = new FakeUserIdentity("Pol");
        var actual = _sut.GetAuthenticatedUser();
        Assert.That(actual, Is.Null);
      }

      [Test]
      public void WhenIdentityIsSetInNancyContext_ReturnsUserFromIdentity() {
        var user = new User();
        var identity = new VoterIdentity(user);
        _nancyContext.CurrentUser = identity;

        var actual = _sut.GetAuthenticatedUser();
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(user));
      }
    }

    void ConfigureSessionFromContextResolver_ToReturn(ISession session) {
      A.CallTo(() => _nancySessionFromNancyContextResolver.ResolveSession(A<NancyContext>._))
       .Returns(session);
    }

    void ConfigureVoterIdentityFactory_ToReturn(User user, VoterIdentity identity) {
      A.CallTo(() => _voterIdentityFactory.Create(user))
       .Returns(identity);
    }
  }
}