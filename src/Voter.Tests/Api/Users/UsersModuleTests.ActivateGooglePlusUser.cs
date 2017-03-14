using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users {
  public partial class UsersModuleTests {
    public class ActivateGooglePlusUser : UsersModuleTests {
      string _validPath;
      string _validIdToken;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validIdToken = "JDoe";
        _validPath = "api/user/activate/googleplus";
      }

      [Test]
      public void AcceptsUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      }

      [Test]
      public void ParsesRequestCorrectly() {
        ActivateGooglePlusUserRequest interceptedRequest = null;
        A.CallTo(() => _activateGooglePlusUserHandler.Handle(A<ActivateGooglePlusUserRequest>._))
         .Invokes(call => interceptedRequest = call.GetArgument<ActivateGooglePlusUserRequest>(0))
         .Returns(true);

        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);

        Post();

        var expected = new ActivateGooglePlusUserRequest {
          IdToken = "JDoe",
          SecurityContext = securityContext
        };
        interceptedRequest.ShouldBeEquivalentTo(expected);
      }

      [Test]
      public void DelegatesControlToInnerHandler() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        A.CallTo(() => _activateGooglePlusUserHandler.Handle(A<ActivateGooglePlusUserRequest>._)).Returns(true);

        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        response.Body.DeserializeJson<bool>().Should().BeTrue();
      }

      BrowserResponse Post(string body = null) {
        return _browser.Post(_validPath,
          with => { with.Body(body ?? _validIdToken, "application/octet-stream; charset=utf-8"); });
      }
    }
  }
}