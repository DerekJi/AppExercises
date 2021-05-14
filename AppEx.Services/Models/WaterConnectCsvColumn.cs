using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class WaterConnectCsvColumn
    {
        /// <summary>
        /// The column name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Indicate how to calculate
        /// </summary>
        public ColumnCalcRule CalcRule { get; set; }

        /// <summary>
        /// Indicates the columns for calculation
        /// </summary>
        public List<string> CalcColumns { get; set; }
    }
}
