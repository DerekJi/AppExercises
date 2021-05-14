using AppEx.Services.CSV;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppEx.Tests.CsvServices
{
    public class WaterConnectServiceTests : TestFixtureBed
    {
        private ICsvService localService { get; set; }

        public WaterConnectServiceTests(DependencyInjectionsFixture di, ITestOutputHelper testOutputHelper)
            : base(di, testOutputHelper)
        {
            localService = (ICsvService)localServiceProvider.GetService(typeof(ICsvService));
        }

        [Fact]
        public async Task FetchStreamAsync_Test()
        {
            var stream = await localService.FetchStreamAsync();
            Assert.NotNull(stream);

            using (var reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                Assert.NotNull(text);
            }
        }

        [Fact]
        public List<ExpandoObject> ConvertToRecords_Test()
        {
            var sampleCsv = "CsvServices\\SampleCsv\\file.csv";
            using (var reader = new StreamReader(sampleCsv))
            {
                var records = localService.ConvertToRecords(reader);
                Assert.NotNull(records);
                Assert.True(records.Count > 0);

                return records;
            }
        }

        [Fact]
        public List<ExpandoObject> TransformRecord_Test()
        {
            var records = ConvertToRecords_Test();

            var newItems = new List<ExpandoObject>();
            if (records?.Count > 0)
            {
                records.ForEach(item =>
                {
                    var newItem = localService.TransformRecord(item as dynamic);

                    localService.RemoveColumns().ForEach(column =>
                    {
                        var contains = ((IDictionary<string, object>)newItem).ContainsKey(column);
                        Assert.False(contains);
                    });

                    localService.NewColumns().ForEach(column =>
                    {
                        var contains = ((IDictionary<string, object>)newItem).ContainsKey(column);
                        Assert.True(contains);
                    });

                    newItems.Add(newItem);
                });
            }

            return newItems;
        }

        [Fact]
        public void SaveAs_Test()
        {
            var records = TransformRecord_Test();
            var filename = "test.csv";
            var fullpath = localService.SaveAs(records, filename, true);
            Assert.NotNull(fullpath);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task FetchRecordsAsync_Test(bool requireTransformed)
        {
            var records = await localService.FetchRecordsAsync(requireTransformed);
            Assert.NotNull(records);
        }
    }
}
