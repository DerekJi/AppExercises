using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class WaterConnectOptions
    {
        /// <summary>
        /// Specify the key name in appsettings.json
        /// </summary>
        public string AppSettingKey { get; } = "WaterConnect";

        /// <summary>
        /// URL of the CSV file
        /// </summary>
        public string CsvUrl { get; set; }

        /// <summary>
        /// Columns to be removed
        /// </summary>
        public List<string> RemoveColumns { get; set; }

        /// <summary>
        /// Columns to be added
        /// </summary>
        public List<WaterConnectCsvColumn> NewColumns { get; set; }

        /// <summary>
        /// The directory to accommodate transformed csv files
        /// </summary>
        public string OutputDir { get; set; }
    }
}
