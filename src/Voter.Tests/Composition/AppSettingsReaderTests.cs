using System;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Composition {
  [TestFixture]
  public class AppSettingsReaderTests {
    AppSettingsReader _sut;
    System.Configuration.Configuration _webConfig;

    [SetUp]
    public virtual void SetUp() {
      _webConfig = WebConfigLoaderForUnitTests.LoadWebConfigForUnitTests();
      _sut = new AppSettingsReader(_webConfig);
    }

    [TestFixture]
    public class Construction : AppSettingsReaderTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        Assert.Throws<ArgumentNullException>(() => new AppSettingsReader(null));
      }
    }

    [TestFixture]
    public class ReadAppSetting : AppSettingsReaderTests {
      [Test]
      public void GivenNullKey_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.ReadAppSetting(null));
      }

      [Test]
      public void WhenSettingIsNotAvailable_ReturnsNull() {
        var keyThatDoesNotExist = Guid.NewGuid().ToString();
        var actual = _sut.ReadAppSetting(keyThatDoesNotExist);
        actual.Should().BeNull();
      }

      [Test]
      public void WhenSettingIsAvailable_ReturnsValueOfSetting() {
        var keyThatExists = "owin:AppStartup";
        var actual = _sut.ReadAppSetting(keyThatExists);
        actual.Should().Be(_webConfig.AppSettings.Settings[keyThatExists].Value);
      }
    }

    [TestFixture]
    public class ReadConnectionString : AppSettingsReaderTests {
      [Test]
      public void GivenNullName_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.ReadConnectionString(null));
      }

      [Test]
      public void WhenConnectionstringIsNotAvailable_ReturnsNull() {
        var keyThatDoesNotExist = Guid.NewGuid().ToString();
        var actual = _sut.ReadConnectionString(keyThatDoesNotExist);
        actual.Should().BeNull();
      }

      [Test]
      public void WhenConnectionStringIsAvailable_ReturnsValueOfSetting() {
        var keyThatExists = "Voter";
        var actual = _sut.ReadConnectionString(keyThatExists);
        actual.Should().Be(_webConfig.ConnectionStrings.ConnectionStrings[keyThatExists]);
      }
    }
  }
}