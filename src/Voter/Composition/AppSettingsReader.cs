using System;
using System.Configuration;

namespace DavidLievrouw.Voter.Composition {
  public class AppSettingsReader : IAppSettingsReader {
    readonly System.Configuration.Configuration _configuration;

    public AppSettingsReader(System.Configuration.Configuration configuration) {
      if (configuration == null) throw new ArgumentNullException(nameof(configuration));
      _configuration = configuration;
    }

    public string ReadAppSetting(string key) {
      if (key == null) throw new ArgumentNullException(nameof(key));
      var setting = _configuration.AppSettings.Settings[key];
      return setting?.Value;
    }

    public ConnectionStringSettings ReadConnectionString(string name) {
      if (name == null) throw new ArgumentNullException(nameof(name));
      var setting = _configuration.ConnectionStrings.ConnectionStrings[name];
      return setting;
    }
  }
}