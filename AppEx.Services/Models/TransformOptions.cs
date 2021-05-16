using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class TransformOptions
    {
        /// <summary>
        /// Columns to be removed
        /// </summary>
        public List<string> RemoveColumns { get; set; }

        /// <summary>
        /// Columns to be added
        /// </summary>
        public List<CsvWaterConnectColumn> NewColumns { get; set; }

    }
}
