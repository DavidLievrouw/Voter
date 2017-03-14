using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users {
  public partial class UsersModuleTests {
    public class DisconnectGooglePlusUser : UsersModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validPath = "api/user/disconnect/googleplus";
      }

      [Test]
      public void DoesNotAcceptUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
      }

      [Test]
      public void ParsesRequestCorrectly() {
        DisconnectGooglePlusUserRequest interceptedRequest = null;
        A.CallTo(() => _disconnectGooglePlusUserHandler.Handle(A<DisconnectGooglePlusUserRequest>._))
         .Invokes(call => interceptedRequest = call.GetArgument<DisconnectGooglePlusUserRequest>(0))
         .Returns(true);

        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);

        Post();

        var expected = new DisconnectGooglePlusUserRequest {
          SecurityContext = securityContext
        };
        interceptedRequest.ShouldBeEquivalentTo(expected);
      }

      [Test]
      public void DelegatesControlToInnerHandler() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        A.CallTo(() => _disconnectGooglePlusUserHandler.Handle(A<DisconnectGooglePlusUserRequest>._)).Returns(true);

        var response = Post();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Body.DeserializeJson<bool>().Should().BeTrue();
      }

      BrowserResponse Post() {
        return _browser.Post(_validPath);
      }
    }
  }
}