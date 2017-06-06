using System;
using System.IO;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Common {
  public class UnclosableStreamWrapperTests {
    MemoryStream _baseStream;
    UnclosableStreamWrapper _sut;
    
    [SetUp]
    public void SetUp() {
      _baseStream = new MemoryStream(new byte[] { 1, 2, 3 });
      _sut = new UnclosableStreamWrapper(_baseStream);
    }

    [Test]
    public void GivenNullBaseStream_Throws() {
      Assert.Throws<ArgumentNullException>(() => new UnclosableStreamWrapper(null));
    }

    [Test]
    public void StreamIsNotClosed_AfterCloseAttempt() {
      _sut.Close();
      Assert.DoesNotThrow(() => _sut.ReadByte());
    }

    [Test]
    public void StreamIsNotClosed_AfterDisposeAttempt() {
      _sut.Dispose();
      Assert.DoesNotThrow(() => _sut.ReadByte());
    }
  }
}