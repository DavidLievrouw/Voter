using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users {
  public partial class UsersModuleTests {
    public class LoginLocalUser : UsersModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validPath = "api/user/login/local";
      }

      [Test]
      public void AcceptsUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      }

      [Test]
      public void ParsesRequestCorrectly() {
        LoginLocalUserRequest interceptedRequest = null;
        A.CallTo(() => _loginLocalUserHandler.Handle(A<LoginLocalUserRequest>._))
         .Invokes(call => interceptedRequest = call.GetArgument<LoginLocalUserRequest>(0))
         .Returns(true);

        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);

        Post();

        var expected = new LoginLocalUserRequest {
          Login = "JDoe",
          Password = "ThePassword",
          SecurityContext = securityContext
        };
        interceptedRequest.ShouldBeEquivalentTo(expected);
      }

      [Test]
      public void GivenUnparseableRequest_ReturnsBadRequest() {
        var response = Post("SomeInvalidJsonString");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _loginLocalUserHandler
           .Handle(A<LoginLocalUserRequest>._))
         .MustNotHaveHappened();
      }

      [Test]
      public void GivenMissingBodyInRequest_CallsInnerHandlerWithEmptyRequest() {
        LoginLocalUserRequest interceptedRequest = null;
        A.CallTo(() => _loginLocalUserHandler.Handle(A<LoginLocalUserRequest>._))
         .Invokes(call => interceptedRequest = call.GetArgument<LoginLocalUserRequest>(0))
         .Returns(true);

        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);

        var response = Post(null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var expected = new LoginLocalUserRequest {
          Login = null,
          Password = null,
          SecurityContext = securityContext
        };
        interceptedRequest.ShouldBeEquivalentTo(expected);
      }

      [Test]
      public void DelegatesControlToInnerHandler() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        A.CallTo(() => _loginLocalUserHandler.Handle(A<LoginLocalUserRequest>._)).Returns(true);

        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        response.Body.DeserializeJson<bool>().Should().BeTrue();
      }

      BrowserResponse Post(string body = ValidJsonString) {
        return _browser.Post(_validPath,
          with => { with.Body(body, "application/json"); });
      }

      const string ValidJsonString = "{ 'Login':'JDoe', 'Password':'ThePassword' }";
    }
  }
}