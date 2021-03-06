﻿using DavidLievrouw.Voter.Api.Models;
using DavidLievrouw.Voter.App.Login.Models;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App {
  [TestFixture]
  public partial class AppModuleTests {
    [TestFixture]
    public class GetLoginView : AppModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _validPath = "/";
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
        var model = new LoginViewModel {ApplicationInfo = "ApplicationInfo"};
        A.CallTo(() => _loginHandler.Handle(A<NancyContext>._)).Returns(model);

        var response = Get();

        var actualViewModel = response.GetModel<LoginViewModel>();
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