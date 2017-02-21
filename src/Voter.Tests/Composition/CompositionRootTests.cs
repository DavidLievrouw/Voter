using System;
using System.IO;
using System.Web.Configuration;
using Autofac;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Handlers;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Configuration;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security;
using DavidLievrouw.Voter.Security.Nancy;
using DavidLievrouw.Voter.Security.Nancy.SessionHijacking;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Composition {
  [TestFixture]
  public class CompositionRootTests {
    IContainer _sut;

    [SetUp]
    public virtual void SetUp() {
      var rootPathProvider = new VoterRootPathProvider();
      var webConfigFile = new FileInfo(Path.Combine(rootPathProvider.GetRootPath(), "web.config"));
      var virtualDirectoryMapping = new VirtualDirectoryMapping(webConfigFile.DirectoryName, true, webConfigFile.Name);
      var webConfigurationFileMap = new WebConfigurationFileMap();
      webConfigurationFileMap.VirtualDirectories.Add("/", virtualDirectoryMapping);
      var configuration = WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, "/");
      _sut = CompositionRoot.Compose(configuration);
    }

    [TestCase(typeof(IUserFromSessionResolver))]
    [TestCase(typeof(IVoterIdentityFactory))]
    [TestCase(typeof(INancyIdentityFromContextAssigner))]
    [TestCase(typeof(IAntiSessionHijackLogic))]
    public void ShouldBeRegistered(Type serviceType) {
      object actualResult = null;
      Assert.DoesNotThrow(() => actualResult = _sut.Resolve(serviceType));
      Assert.That(actualResult, Is.Not.Null);
      Assert.That(actualResult, Is.InstanceOf(serviceType));
    }

    [TestCase(typeof(JsonSerializer), typeof(CustomJsonSerializer))]
    [TestCase(typeof(ICustomJsonSerializer), typeof(CustomJsonSerializer))]
    public void ShouldBeRegisteredCorrectly(Type serviceType, Type instanceType) {
      object actualResult = null;
      Assert.DoesNotThrow(() => actualResult = _sut.Resolve(serviceType));
      Assert.That(actualResult, Is.Not.Null);
      Assert.That(actualResult, Is.InstanceOf(instanceType));
    }

    [TestCase(typeof(IHandler<LoginRequest, bool>), typeof(RequestValidationAwareHandler<LoginRequest, bool>))]
    [TestCase(typeof(IHandler<GetCurrentUserRequest, User>), typeof(RequestValidationAwareHandler<GetCurrentUserRequest, User>))]
    [TestCase(typeof(INancySecurityContextFactory), typeof(NancySecurityContextFactory))]
    public void RegistersDecoratorsCorrectly(Type requestedType, Type expectedType) {
      object actualResult = null;
      Assert.DoesNotThrow(() => actualResult = _sut.Resolve(requestedType));
      Assert.That(actualResult, Is.Not.Null);
      Assert.That(actualResult, Is.InstanceOf(expectedType));
    }
  }
}