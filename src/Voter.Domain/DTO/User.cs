using System;

namespace DavidLievrouw.Voter.Domain.DTO {
  public class User {
    public Login Login { get; set; }
    public Guid UniqueId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LastNamePrefix { get; set; }
    public Password Password { get; set; }
    public ExternalCorrelationId ExternalCorrelationId { get; set; }
    public UserType Type { get; set; }
    public OAuthToken OAuthToken { get; set; }
  }
}