using AppEx.Services.CSV;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppEx.Tests.CsvServices
{
    public class CsvServiceTests : TestFixtureBed
    {
        private ICsvService localService { get; set; }

        public CsvServiceTests(DependencyInjectionsFixture di, ITestOutputHelper testOutputHelper)
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
        public async Task<List<ExpandoObject>> TransformRecord_Test()
        {
            var records = await localService.FetchRecordsAsync(true);

            Assert.NotNull(records);

            return records;
        }

        [Fact]
        public async Task SaveAs_Test()
        {
            var records = await TransformRecord_Test();
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
