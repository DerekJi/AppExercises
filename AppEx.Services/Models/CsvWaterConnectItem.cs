namespace AppEx.Services.Models
{
    public class CsvWaterConnectItem
    {
        public string DHNO { get; set; }

        public string network { get; set; }

        public string Unit_Number { get; set; }

        public string Aquifer { get; set; }
        public string Easting { get; set; }
        public string Northing { get; set; }
        public string Zone { get; set; }
        public string Unit_No { get; set; }
        public string Obs_No { get; set; }
        public string obs_date { get; set; }
        public string swl { get; set; }
        public string rswl { get; set; }
        public string pressure { get; set; }
        public string temperature { get; set; }
        public string dry_ind { get; set; }
        public string anom_ind { get; set; }
        public string pump_ind { get; set; }
        public string measured_during { get; set; }
        public string data_source { get; set; }
        public string Comments { get; set; }
    }
}
