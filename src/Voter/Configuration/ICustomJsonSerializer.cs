namespace DavidLievrouw.Voter.Configuration {
  public interface ICustomJsonSerializer {
    string Serialize(object model);
    T Deserialize<T>(string reader);
  }
}