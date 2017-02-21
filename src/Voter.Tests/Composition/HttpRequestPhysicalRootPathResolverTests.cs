using System.Reflection;
using System.Web;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Composition {
  [TestFixture]
  public class HttpRequestPhysicalRootPathResolverTests {
    HttpRuntimePhysicalRootPathResolver _sut;
    const string ApplicationPath = "c:\\test\\ApplicationPath";

    [SetUp]
    public void SetUp() {
      InitHttpRuntimeWithApplicationPath();
      _sut = new HttpRuntimePhysicalRootPathResolver();
    }

    [Test]
    public void DeterminesRootPath_BasedOnRequestData() {
      var actual = _sut.ResolvePhysicalRootPath();
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(ApplicationPath));
    }

    static void InitHttpRuntimeWithApplicationPath() {
      var field = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static);
      var appDomainAppPath = field.GetValue(null).GetType().GetField("_appDomainAppPath", BindingFlags.NonPublic | BindingFlags.Instance);
      appDomainAppPath.SetValue(field.GetValue(null), ApplicationPath);
    }
  }
}