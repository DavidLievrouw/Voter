namespace DavidLievrouw.Voter.Security {
  public interface ISession {
    object this[string name] { get; set; }
    void Abandon();
  }
}