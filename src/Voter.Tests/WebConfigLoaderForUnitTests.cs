using System.IO;
using System.Web.Configuration;

namespace DavidLievrouw.Voter {
  public static class WebConfigLoaderForUnitTests {
    public static System.Configuration.Configuration LoadWebConfigForUnitTests() {
      var unitTestAssembly = typeof(WebConfigLoaderForUnitTests).Assembly;
      var directoryName = Path.GetDirectoryName(unitTestAssembly.Location);
      var assemblyPath = directoryName.Replace(@"file:\", string.Empty);
      var webConfigFile = new FileInfo(Path.Combine(assemblyPath, unitTestAssembly.GetName().Name + ".dll.config"));
      var virtualDirectoryMapping = new VirtualDirectoryMapping(webConfigFile.DirectoryName, true, webConfigFile.Name);
      var webConfigurationFileMap = new WebConfigurationFileMap();
      webConfigurationFileMap.VirtualDirectories.Add("/", virtualDirectoryMapping);
      return WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, "/");
    }
  }
}