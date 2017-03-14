using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users {
  public partial class UsersModuleTests {
    public class LoginGooglePlusUser : UsersModuleTests {
      string _validPath;
      string _validCode;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validCode = "JDoe";
        _validPath = "api/user/login/googleplus";
      }

      [Test]
      public void AcceptsUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      }

      [Test]
      public void ParsesRequestCorrectly() {
        LoginGooglePlusUserRequest interceptedRequest = null;
        A.CallTo(() => _loginGooglePlusUserHandler.Handle(A<LoginGooglePlusUserRequest>._))
         .Invokes(call => interceptedRequest = call.GetArgument<LoginGooglePlusUserRequest>(0))
         .Returns(true);

        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);

        Post();

        var expected = new LoginGooglePlusUserRequest {
          Code = "JDoe",
          SecurityContext = securityContext
        };
        interceptedRequest.ShouldBeEquivalentTo(expected);
      }

      [Test]
      public void DelegatesControlToInnerHandler() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        A.CallTo(() => _loginGooglePlusUserHandler.Handle(A<LoginGooglePlusUserRequest>._)).Returns(true);

        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        response.Body.DeserializeJson<bool>().Should().BeTrue();
      }

      BrowserResponse Post(string body = null) {
        return _browser.Post(_validPath,
          with => { with.Body(body ?? _validCode, "application/octet-stream; charset=utf-8"); });
      }
    }
  }
}