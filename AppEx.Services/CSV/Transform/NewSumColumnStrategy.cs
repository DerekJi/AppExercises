using AppEx.Services.Models;
using System.Collections.Generic;

namespace AppEx.Services.CSV.Transform
{
    public class NewSumColumnStrategy : ITransformStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="expando"></param>
        /// <returns></returns>
        public dynamic Transform(WaterConnectOptions options, dynamic expando)
        {
            // Add Columns
            if (options?.NewColumns?.Count > 0)
            {
                options.NewColumns.ForEach(column =>
                {
                    if (column.CalcRule == ColumnCalcRule.Sum)
                    {
                        var sum = SumOfColumns(expando, column.CalcColumns);
                        ((IDictionary<string, object>)expando).Add(column.ColumnName, sum);
                    }
                });
            }

            return expando;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected double SumOfColumns(dynamic expando, List<string> columns)
        {
            double sum = 0;
            if (columns?.Count > 0)
            {
                columns.ForEach(column =>
                {
                    if (((IDictionary<string, object>)expando).ContainsKey(column))
                    {
                        var obj = ((IDictionary<string, object>)expando)[column];
                        double.TryParse(obj.ToString(), out var value);
                        sum += value;
                    }
                });
            }
            return sum;
        }
    }
}
