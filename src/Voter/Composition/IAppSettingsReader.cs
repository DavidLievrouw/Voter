using System.Configuration;

namespace DavidLievrouw.Voter.Composition {
  public interface IAppSettingsReader {
    string ReadAppSetting(string key);
    ConnectionStringSettings ReadConnectionString(string name);
  }
}