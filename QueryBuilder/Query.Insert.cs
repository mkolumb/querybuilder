using System;
using System.Collections.Generic;
using System.Linq;
using SqlKata.Net6.Clauses;
using SqlKata.Net6.Extensions;

namespace SqlKata.Net6
{
    public partial class Query
    {
        public Query AsInsert(object data, bool returnId = false)
        {
            var propertiesKeyValues = BuildKeyValuePairsFromObject(data);

            return AsInsert(propertiesKeyValues, returnId);
        }

        public Query AsInsert(IEnumerable<string> columns, IEnumerable<object> values)
        {
            var columnsList = columns?.ToList();
            var valuesList = values?.ToList();

            if ((columnsList?.Count ?? 0) == 0 || (valuesList?.Count ?? 0) == 0)
            {
                throw new InvalidOperationException($"{nameof(columns)} and {nameof(values)} cannot be null or empty");
            }

            if (columnsList.Count != valuesList.Count)
            {
                throw new InvalidOperationException($"{nameof(columns)} and {nameof(values)} cannot be null or empty");
            }

            Method = "insert";

            ClearComponent("insert").AddComponent("insert", new InsertClause
            {
                Data = columnsList.MergeKeysAndValues(valuesList)
            });

            return this;
        }

        public Query AsInsert(IEnumerable<KeyValuePair<string, object>> values, bool returnId = false)
        {
            if (values == null || values.Any() == false)
            {
                throw new InvalidOperationException($"{values} argument cannot be null or empty");
            }

            Method = "insert";

            ClearComponent("insert").AddComponent("insert", new InsertClause
            {
                Data = values.CreateDictionary(),
                ReturnId = returnId,
            });

            return this;
        }

        /// <summary>
        /// Produces insert multi records
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rowsValues"></param>
        /// <returns></returns>
        public Query AsInsert(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> rowsValues)
        {
            var columnsList = columns?.ToList();
            var valuesCollectionList = rowsValues?.ToList();

            if ((columnsList?.Count ?? 0) == 0 || (valuesCollectionList?.Count ?? 0) == 0)
            {
                throw new InvalidOperationException($"{nameof(columns)} and {nameof(rowsValues)} cannot be null or empty");
            }

            Method = "insert";

            ClearComponent("insert");

            foreach (var values in valuesCollectionList)
            {
                var valuesList = values.ToList();
                if (columnsList.Count != valuesList.Count)
                {
                    throw new InvalidOperationException($"{nameof(columns)} count should be equal to each {nameof(rowsValues)} entry count");
                }

                AddComponent("insert", new InsertClause
                {
                    Data = columnsList.MergeKeysAndValues(valuesList)
                });
            }

            return this;
        }

        /// <summary>
        /// Produces insert multi records
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Query AsInsert(IEnumerable<IEnumerable<KeyValuePair<string, object>>> data)
        {
            if (data == null || !data.Any())
            {
                throw new InvalidOperationException($"{nameof(data)} cannot be null or empty");
            }

            Method = "insert";

            ClearComponent("insert");

            foreach (var item in data)
            {
                var row = item.CreateDictionary();

                if (row.Keys.Count != row.Values.Count)
                {
                    throw new InvalidOperationException($"{nameof(row.Keys)} count should be equal to each {nameof(row.Values)} entry count");
                }

                AddComponent("insert", new InsertClause
                {
                    Data = row
                });
            }

            return this;
        }

        /// <summary>
        /// Produces insert from subquery
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public Query AsInsert(IEnumerable<string> columns, Query query)
        {
            Method = "insert";

            ClearComponent("insert").AddComponent("insert", new InsertQueryClause
            {
                Columns = columns.ToList(),
                Query = query.Clone(),
            });

            return this;
        }
    }
}
