using System;
using System.IO;
using Nancy;

namespace DavidLievrouw.Voter.Configuration {
  public class VoterRootPathProvider : IRootPathProvider {
    readonly string _rootPath;

    public VoterRootPathProvider() {
      var directoryName = Path.GetDirectoryName(typeof(Startup).Assembly.Location);

      if (directoryName != null) {
        var subDirs = Path.Combine("..", "..", "..");
        if (Isx86Process()) subDirs = Path.Combine(subDirs, "..");
        var assemblyPath = directoryName.Replace(@"file:\", string.Empty);
        _rootPath = Path.Combine(assemblyPath, subDirs, "Voter");
      }
    }

    public string GetRootPath() {
      return _rootPath;
    }

    static bool Isx86Process() {
      return IntPtr.Size == 4;
    }
  }
}