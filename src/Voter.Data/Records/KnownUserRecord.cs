using System;

namespace DavidLievrouw.Voter.Data.Records {
  public class KnownUserRecord {
    public string Login { get; set; }
    public Guid UniqueId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LastNamePrefix { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public char Type { get; set; }
    public string ExternalCorrelationId { get; set; }
  }
}