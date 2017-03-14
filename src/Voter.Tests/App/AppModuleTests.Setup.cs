﻿using DavidLievrouw.Utils;
using DavidLievrouw.Utils.ForTesting.DotNet;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Voter.App.Login.Models;
using DavidLievrouw.Voter.Configuration;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App {
  [TestFixture]
  public partial class AppModuleTests {
    AppModule _sut;
    Browser _browser;
    AppBootstrapper _bootstrapper;
    IHandler<NancyContext, LoginViewModel> _loginHandler;

    [SetUp]
    public virtual void SetUp() {
      _loginHandler = _loginHandler.Fake();
      _sut = new AppModule(_loginHandler);
      _bootstrapper = new AppBootstrapper(
        with => {
          with.Module(_sut);
          with.RootPathProvider(new VoterRootPathProvider());
        },
        enableViewSupportWhichMakesTheUnitTestsReallySlow: true);
      _browser = new Browser(_bootstrapper, to => to.Accept(new MediaRange("text/html")));
    }

    [TestFixture]
    public class MethodName : AppModuleTests {
      [Test]
      public void ConstructorTests() {
        Assert.That(_sut.NoDependenciesAreOptional());
      }
    }
  }
}