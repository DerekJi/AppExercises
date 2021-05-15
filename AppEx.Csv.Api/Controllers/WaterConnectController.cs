using AppEx.Services.CSV;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppEx.Csv.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaterConnectController : ControllerBase
    {
        private readonly ILogger<WaterConnectController> _logger;
        private readonly ICsvService _csvService;

        public WaterConnectController(
            ICsvService csvService,
            ILogger<WaterConnectController> logger)
        {
            _csvService = csvService;
            _logger = logger;
        }

        [HttpGet("CsvFile")]
        public async Task<ActionResult> GetWaterConnectCsv()
        {
            var content = await _csvService.FetchRecordsAsByteArrayAsync(true);

            var contentType = "application/force-download";
            var fileName = "TransformedFile.csv";

            return File(content, contentType, fileName);
        }
    }
}
