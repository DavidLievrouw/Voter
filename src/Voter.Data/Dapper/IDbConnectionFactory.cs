using System.Data;

namespace DavidLievrouw.Voter.Data.Dapper {
  public interface IDbConnectionFactory {
    IDbConnection OpenConnection();
  }
}