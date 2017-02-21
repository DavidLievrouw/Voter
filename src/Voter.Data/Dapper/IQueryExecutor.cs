using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.Dapper {
  public interface IQueryExecutor {
    IQueryExecutor NewQuery(string sql);

    IQueryExecutor WithParameters(object parameters);
    IQueryExecutor WithParameters(IEnumerable<KeyValuePair<string, object>> parameters);
    IQueryExecutor WithCommandType(CommandType commandType);

    IEnumerable<TResult> Execute<TResult>();
    TResult ExecuteScalar<TResult>();
    IEnumerable<TResult> ExecuteWithAnonymousResult<TResult>(TResult anonymousPrototype);
    int Execute();

    Task<IEnumerable<TResult>> ExecuteAsync<TResult>();
    Task<TResult> ExecuteScalarAsync<TResult>();
    Task<IEnumerable<TResult>> ExecuteWithAnonymousResultAsync<TResult>(TResult anonymousPrototype);
    Task<int> ExecuteAsync();

    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TResult>(Func<TFirst, TSecond, TThird, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> mapper, string splitOn = "Id");

    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TResult>(Func<TFirst, TSecond, TThird, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> mapper, string splitOn = "Id");
  }
}