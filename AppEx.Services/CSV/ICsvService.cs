using AppEx.Services.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace AppEx.Services.CSV
{
    public interface ICsvService
    {
        List<string> RemoveColumns();

        List<string> NewColumns();

        Task<Stream> FetchStreamAsync();

        List<ExpandoObject> ConvertToRecords(Stream stream);

        List<ExpandoObject> ConvertToRecords(StreamReader reader);

        Task<List<ExpandoObject>> FetchRecordsAsync(bool requireTransform);

        Task<Byte[]> FetchRecordsAsByteArrayAsync(bool requireTransform);

        string SaveAs(List<ExpandoObject> records, string filename, bool force);

        ExpandoObject TransformRecord(CsvWaterConnectItem csvItem);

        ExpandoObject TransformRecord(dynamic expando);

        List<ExpandoObject> TransformRecords(List<ExpandoObject> records);
    }
}
