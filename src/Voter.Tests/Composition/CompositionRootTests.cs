using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using Autofac;
using DavidLievrouw.Utils;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Voter.Api.Handlers;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Configuration;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security;
using DavidLievrouw.Voter.Security.Nancy;
using DavidLievrouw.Voter.Security.Nancy.SessionHijacking;
using Nancy;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Composition {
  [TestFixture]
  public class CompositionRootTests {
    IPhysicalRootPathResolver _physicalRootPathResolver;
    IContainer _sut;

    [SetUp]
    public virtual void SetUp() {
      _physicalRootPathResolver = _physicalRootPathResolver.Fake();
      var rootPathProvider = new VoterRootPathProvider();
      var webConfigFile = new FileInfo(Path.Combine(rootPathProvider.GetRootPath(), "web.config"));
      var virtualDirectoryMapping = new VirtualDirectoryMapping(webConfigFile.DirectoryName, true, webConfigFile.Name);
      var webConfigurationFileMap = new WebConfigurationFileMap();
      webConfigurationFileMap.VirtualDirectories.Add("/", virtualDirectoryMapping);
      var configuration = WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, "/");
      _sut = CompositionRoot.Compose(configuration, _physicalRootPathResolver);
    }

    [Test]
    public void CanRegisterAllNancyFxModules() {
      var apiAssembly = typeof(Startup).Assembly;
      var nancyModules = apiAssembly.GetTypes()
                                    .Where(t => t.IsAssignableTo<INancyModule>())
                                    .Where(t => t.IsClass && !t.IsAbstract);

      var nancyModuleDependencies = nancyModules.SelectMany(nancyModule => {
        var ctor = nancyModule.GetConstructors(BindingFlags.Public | BindingFlags.Instance).SingleOrDefault();
        return ctor?.GetParameters() ?? new ParameterInfo[0];
      });

      nancyModuleDependencies.ForEach(nancyModuleDependency => {
        object actualResult = null;
        Assert.DoesNotThrow(() => actualResult = _sut.Resolve(nancyModuleDependency.ParameterType));
        Assert.That(actualResult, Is.Not.Null);
        Assert.That(actualResult, Is.AssignableTo(nancyModuleDependency.ParameterType));
      });
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

    [TestCase(typeof(IHandler<LoginLocalUserRequest, bool>), typeof(RequestValidationAwareHandler<LoginLocalUserRequest, bool>))]
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