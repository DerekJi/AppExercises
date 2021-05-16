using Newtonsoft.Json;

namespace AppEx.Services.Models
{
    public class WeatherRecordItem
    {
        [JsonProperty("sort_order")]
        public int? SortOrder { get; set; }

        [JsonProperty("wmo")]
        public WeatherWmo Wmo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("history_product")]
        public string HistoryProduct { get; set; }

        [JsonProperty("local_date_time")]
        public string LocalDateTime { get; set; }

        [JsonProperty("local_date_time_full")]
        public string LocalDateTimeFull { get; set; }

        [JsonProperty("aifstime_utc")]
        public string AifsTimeUtc { get; set; }

        [JsonProperty("lat")]
        public double? Latitude { get; set; }

        [JsonProperty("lon")]
        public double? Longtitude { get; set; }

        [JsonProperty("apparent_t")]
        public double? ApparentT { get; set; }

        [JsonProperty("cloud")]
        public string Cloud { get; set; }

        [JsonProperty("cloud_base_m")]
        public int? CloudBaseM { get; set; }

        [JsonProperty("cloud_oktas")]
        public int? CloudOktas { get; set; }

        [JsonProperty("cloud_type_id")]
        public int? CloudTypeId { get; set; }

        [JsonProperty("cloud_type")]
        public string CloudType { get; set; }

        [JsonProperty("delta_t")]
        public double? DeltaT { get; set; }

        [JsonProperty("gust_kmh")] 
        public int? GustKmh { get; set; }

        [JsonProperty("gust_kt")]
        public int? GustKt { get; set; }

        [JsonProperty("air_temp")]
        public double? AirTemperature { get; set; }

        [JsonProperty("dewpt")]
        public double? DewpPoint { get; set; }

        [JsonProperty("press")]
        public double? Press { get; set; }

        [JsonProperty("press_qnh")]
        public double? PressQnh { get; set; }

        [JsonProperty("press_msl")]
        public double? RressMsl { get; set; }

        [JsonProperty("press_tend")]
        public string PressTend { get; set; }

        [JsonProperty("rain_trace")]
        public string RainTrace { get; set; }

        [JsonProperty("rel_um")]
        public int? RelHum { get; set; }

        [JsonProperty("sea_state")]
        public string SeaState { get; set; }

        [JsonProperty("swell_dir_worded")]
        public string SwellDirWorded { get; set; }

        [JsonProperty("swell_height")]
        public int? SwellHeight { get; set; }

        [JsonProperty("swell_period")]
        public int? SwellPeriod { get; set; }

        [JsonProperty("vis_km")]
        public string VisibilityKm { get; set; }

        [JsonProperty("weather")]
        public string Weather { get; set; }

        [JsonProperty("wind_dir")]
        public string WindDir { get; set; }

        [JsonProperty("wind_spd_kmh")]
        public int? WindSpeedKmh { get; set; }

        [JsonProperty("wind_spd_kt")]
        public int? WindSpeedKt { get; set; }
    }
}
