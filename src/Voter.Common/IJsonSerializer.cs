namespace DavidLievrouw.Voter.Common {
  public interface IJsonSerializer {
    string Serialize(object model);
    T Deserialize<T>(string reader);
  }
}