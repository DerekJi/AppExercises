using AppEx.Core.Attributes;
using AppEx.Core.Extensions;
using AppEx.Services.Models;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AppEx.Services.CSV
{
    [Service(typeof(ICsvService))]
    public class WaterConnectService : ICsvService
    {
        protected IConfiguration Configuration { get; set; }
        protected WaterConnectOptions Options { get; }

        public WaterConnectService(IConfiguration configuration)
        {
            Options = new WaterConnectOptions();
            Configuration = configuration;
            Configuration.GetSection(Options.AppSettingKey).Bind(Options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> RemoveColumns()
        {
            return Options?.RemoveColumns ?? new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> NewColumns()
        {
            return Options?.NewColumns?.Select(x => x.ColumnName).ToList() ?? new List<string>();
        }

        /// <summary>
        /// Fetch remote CSV file and return as Stream
        /// </summary>
        /// <returns>Stream</returns>
        public async Task<Stream> FetchStreamAsync()
        {
            using (var wc = new WebClient())
            {
                var url = new Uri(Options.CsvUrl);
                var stream = await wc.OpenReadTaskAsync(url);
                return stream;
            }
        }

        /// <summary>
        /// Convert the given stream to a list of records
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>List of records</returns>
        public List<ExpandoObject> ConvertToRecords(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return ConvertToRecords(reader);
            }
        }

        /// <summary>
        /// Convert content in the given reader to a list of records
        /// </summary>
        /// <param name="reader">StreamReader</param>
        /// <returns>List of records</returns>
        public List<ExpandoObject> ConvertToRecords(StreamReader reader)
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<WaterConnectCsvItem>().ToList();
                var expandos = new List<ExpandoObject>();
                if (records?.Count > 0)
                {
                    records.ForEach(r =>
                    {
                        dynamic expando = r.ToExpando();
                        expandos.Add(expando);
                    });
                }
                return expandos;
            }
        }

        /// <summary>
        /// Fetch remote CSV file and return a list of records
        /// </summary>
        /// <param name="requireTransform">indicates if return transformed records or not</param>
        /// <returns></returns>
        public async Task<List<ExpandoObject>> FetchRecordsAsync(bool requireTransform)
        {
            var csvStream = await FetchStreamAsync();

            var records = ConvertToRecords(csvStream);

            if (requireTransform && records?.Count > 0)
            {
                return TransformRecords(records);
            }
           
            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="records"></param>
        /// <param name="filename"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public string SaveAs(List<ExpandoObject> records, string filename, bool force)
        {
            if (Options?.OutputDir?.Length > 0 && !Directory.Exists(Options.OutputDir))
            {
                Directory.CreateDirectory(Options.OutputDir);
            }

            var path = Path.Combine(Options.OutputDir, filename);
            if (File.Exists(path))
            {
                if (!force)
                {
                    throw new IOException($"The file {path} exists already.");
                }
                else
                {
                    File.Delete(path);
                }
            }

            using (var writer = new StreamWriter(path))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var transformedRecords = new List<WaterConnectCsvTranformItem>();
                    if (records?.Count > 0)
                    {
                        records.ForEach(item =>
                        {
                            var transformItem = item.ConvertTo<WaterConnectCsvTranformItem>();
                            transformedRecords.Add(transformItem);
                        });
                    }
                    csv.WriteRecords(transformedRecords);
                    return path;
                }
            }            
        }

        /// <summary>
        /// Transform a record based on predefined rules (in appsettings.json)
        /// </summary>
        /// <param name="csvItem">a record in CSV file</param>
        /// <returns>transformed record</returns>
        public ExpandoObject TransformRecord(WaterConnectCsvItem csvItem)
        {
            var expando = csvItem.ToExpando();

            return TransformRecord(expando);
        }

        /// <summary>
        /// Transform all the records based on predefined rules (in appsettings.json)
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<ExpandoObject> TransformRecords(List<ExpandoObject> records)
        {
            var list = new List<ExpandoObject>();
            if (records?.Count > 0)
            {
                records.ForEach(item =>
                {
                    var newItem = TransformRecord(item as dynamic);
                    list.Add(newItem);
                });
            }
            return list;
        }

        /// <summary>
        /// Transform a record based on predefined rules (in appsettings.json)
        /// </summary>
        /// <param name="csvItem">a record in CSV file</param>
        /// <returns>transformed record</returns>
        public ExpandoObject TransformRecord(dynamic expando)
        {
            if (Options != null)
            {
                // Remove Columns
                if (Options.RemoveColumns?.Count > 0)
                {
                    Options.RemoveColumns.ForEach(column =>
                    {
                        ((IDictionary<string, object>)expando).Remove(column);
                    });
                }

                // Add Columns
                if (Options.NewColumns?.Count > 0)
                {
                    Options.NewColumns.ForEach(column =>
                    {
                        if (column.CalcRule == ColumnCalcRule.Sum)
                        {
                            var sum = SumOfColumns(expando, column.CalcColumns);
                            ((IDictionary<string, object>)expando).Add(column.ColumnName, sum);
                        }
                    });
                }
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
