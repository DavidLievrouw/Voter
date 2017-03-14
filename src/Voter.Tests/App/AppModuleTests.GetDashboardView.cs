using System;
using DavidLievrouw.Voter.App.Dashboard.Models;
using DavidLievrouw.Voter.Domain.DTO;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App {
  [TestFixture]
  public partial class AppModuleTests {
    [TestFixture]
    public class GetDashboardView : AppModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = new User {
          UniqueId = Guid.NewGuid(),
          Login = new Domain.DTO.Login {Value = "JDoe"},
        };
        _validPath = "/dashboard";
      }

      [Test]
      public void DoesNotAcceptUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Get();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
      }

      [Test]
      public void ShouldReturnView() {
        var response = Get();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var bodyString = response.Body.AsString();
        Assert.That(string.IsNullOrWhiteSpace(bodyString), Is.False);
      }

      [Test]
      public void ReturnsResultFromHandlerInView() {
        var model = new DashboardViewModel {ApplicationInfo = "ApplicationInfo"};
        A.CallTo(() => _dashboardHandler.Handle(A<NancyContext>._)).Returns(model);

        var response = Get();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var actualViewModel = response.GetModel<DashboardViewModel>();
        actualViewModel.ShouldBeEquivalentTo(model);
      }

      BrowserResponse Get() {
        return Get(_validPath);
      }

      BrowserResponse Get(string path) {
        return _browser.Get(path);
      }
    }
  }
}