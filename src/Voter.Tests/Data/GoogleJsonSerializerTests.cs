using System;
using System.IO;
using System.Runtime.Serialization;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data {
  [TestFixture]
  public class GoogleJsonSerializerTests {
    GoogleJsonSerializer _sut;

    [SetUp]
    public void SetUp() {
      _sut = new GoogleJsonSerializer();
    }

    [TestCase("image/png")]
    [TestCase("text/html")]
    public void CannotSerializeNonJsonContentType(string contentType) {
      _sut.CanSerialize(contentType).Should().BeFalse();
    }

    [Test]
    public void CanRoundtripToJson() {
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      var serializedData = _sut.Serialize(originalData);
      var deserializedData = _sut.Deserialize<Data>(serializedData);

      deserializedData.Message.Should().Be(originalData.Message);
      deserializedData.Number.Should().Be(originalData.Number);
      deserializedData.AssertionSucceeded.Should().Be(originalData.AssertionSucceeded);
    }

    [Test]
    public void CanSerializeJsonContent() {
      const string contentType = "application/json";
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      using (var memStream = new MemoryStream()) {
        Assert.DoesNotThrow(() => _sut.Serialize(contentType, originalData, memStream));
        memStream.Position.Should().BeGreaterThan(0);
      }
    }

    [TestCase("application/json")]
    [TestCase("application/JSON")]
    [TestCase("application/json-prop")]
    [TestCase("application/JSON-prop")]
    [TestCase("text/json")]
    [TestCase("text/JSON")]
    [TestCase("application/vndTEST+json")]
    [TestCase("application/vndTEST+JSON")]
    [TestCase("application/json;otherdata")]
    [TestCase("application/json; charset=utf-8")]
    public void CanSerializeJsonContentType(string contentType) {
      _sut.CanSerialize(contentType).Should().BeTrue();
    }

    [Test]
    public void ConfiguresJsonExtension() {
      _sut.Extensions.Should().Contain("json");
    }

    [Test]
    public void DeserializeNullString_Fails() {
      Assert.Throws<ArgumentNullException>(() => _sut.Deserialize<Data>(null));
    }

    [Test]
    public void RegisteredCorrectContractResolver() {
      var serializer = (JsonSerializer) _sut;
      serializer.ContractResolver.Should().BeAssignableTo<DefaultContractResolver>();
    }

    [Test]
    public void Serialize_WithInvalidJsonString_Fails() {
      const string invalidJson = "Blah Blah";
      Assert.Throws<JsonReaderException>(() => _sut.Deserialize<Data>(invalidJson));
    }

    [Test]
    public void Serialize_WithNullContentType_Fails() {
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      using (var memStream = new MemoryStream()) {
        Assert.Throws<ArgumentNullException>(() => _sut.Serialize(null, originalData, memStream));
      }
    }

    [Test]
    public void Serialize_WithNullOutputStream_Fails() {
      const string contentType = "text/html";
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};
      Assert.Throws<ArgumentNullException>(() => _sut.Serialize(contentType, originalData, null));
    }

    [Test]
    public void SerializeNonJsonContent_Fails() {
      const string contentType = "text/html";
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      using (var memStream = new MemoryStream()) {
        Assert.Throws<SerializationException>(() => _sut.Serialize(contentType, originalData, memStream));
      }
    }

    [Test]
    public void SerializesNullInputModel_ToNullJson() {
      var json = _sut.Serialize(null);
      json.Should().Be("null");
    }

    [Test]
    public void SerializesToPascalCaseProperties() {
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};
      const string expectedResult = @"{""Message"":""test"",""Number"":42,""AssertionSucceeded"":true}";

      var actual = _sut.Serialize(originalData);

      actual.Should().Be(expectedResult);
    }

    [Test]
    public void SupportsPublicAndPrivatePropertySetters() {
      var originalData = new DataWithPrivateSetters("privateData") {PublicSetter = "publicData"};
      var serializedData = _sut.Serialize(originalData);
      var deserializedData = _sut.Deserialize<DataWithPrivateSetters>(serializedData);
      deserializedData.PublicSetter.Should().Be(originalData.PublicSetter);
      deserializedData.PrivateSetter.Should().Be(originalData.PrivateSetter);
    }

    class Data {
      public string Message { get; set; }
      public int Number { get; set; }
      public bool AssertionSucceeded { get; set; }
    }

    class DataWithPrivateSetters {
      public DataWithPrivateSetters(string privateSetter) {
        PrivateSetter = privateSetter;
      }

      public string PublicSetter { get; set; }
      public string PrivateSetter { get; private set; }
    }
  }
}