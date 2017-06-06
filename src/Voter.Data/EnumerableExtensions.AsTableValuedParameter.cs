using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;

namespace DavidLievrouw.Voter.Data {
  public static class EnumerableExtensions {
    /// <summary>This extension converts an enumerable set to a Dapper TableValuedParameter</summary>
    /// <typeparam name="T">T must be an IEnumerable</typeparam>
    /// <param name="enumerable">List of values</param>
    /// <param name="typeName">The database type name to use</param>
    /// <param name="orderedColumnNames">
    ///   If more than one column in a TableValuedParameter, columns order must match order of columns in the
    ///   TableValuedParameter
    /// </param>
    /// <returns>A custom query parameter (TableValuedParameter)</returns>
    public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(this IEnumerable<T> enumerable, string typeName, IEnumerable<string> orderedColumnNames = null) {
      var dataTable = new DataTable();
      if (typeof(T).IsValueType || typeof(T).FullName.Equals("System.String")) {
        dataTable.Columns.Add(orderedColumnNames == null
          ? "NONAME"
          : orderedColumnNames.First(), typeof(T));
        foreach (var obj in enumerable) {
          dataTable.Rows.Add(obj);
        }
      } else {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var readableProperties = properties.Where(propertyInfo => propertyInfo.CanRead).ToArray();
        if (readableProperties.Length > 1 && orderedColumnNames == null) throw new ArgumentException("Ordered list of column names must be provided when TVP contains more than one column");

        var columnNames = (orderedColumnNames ?? readableProperties.Select(propertyInfo => propertyInfo.Name)).ToArray();
        foreach (var columnName in columnNames) {
          dataTable.Columns.Add(columnName, readableProperties
            .Where(propertyInfo => propertyInfo.Name.Equals(columnName))
            .Select(propertyInfo => Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType)
            .Single());
        }

        foreach (var item in enumerable) {
          dataTable.Rows.Add(columnNames.Select(columnName => readableProperties.Single(propertyInfo => propertyInfo.Name.Equals(columnName)).GetValue(item)).ToArray());
        }
      }
      return dataTable.AsTableValuedParameter(typeName);
    }
  }
}