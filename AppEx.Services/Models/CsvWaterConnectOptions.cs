using System.Collections.Generic;

namespace AppEx.Services.Models
{
    public class CsvWaterConnectOptions: TransformOptions
    {
        /// <summary>
        /// Specify the key name in appsettings.json
        /// </summary>
        public string AppSettingKey { get; } = "WaterConnect";

        /// <summary>
        /// Specify the target file name (without extension)
        /// </summary>
        public string OutputName { get; set; }

        /// <summary>
        /// URL of the CSV file
        /// </summary>
        public string CsvUrl { get; set; }

        /// <summary>
        /// The directory to accommodate transformed csv files
        /// </summary>
        public string OutputDir { get; set; }
    }
}
