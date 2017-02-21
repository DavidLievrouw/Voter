using System;

namespace DavidLievrouw.Voter.Domain.DTO {
  public class User {
    public Login Login { get; set; }
    public Guid UniqueId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LastNamePrefix { get; set; }
    public Password Password { get; set; }
  }
}