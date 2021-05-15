using AppEx.Services.CSV;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppEx.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CsvController : ControllerBase
    {
        private readonly ILogger<CsvController> _logger;
        private readonly ICsvService _service;

        public CsvController(
            ICsvService service,
            ILogger<CsvController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("WaterConnect")]
        public async Task<ActionResult> GetWaterConnectCsv()
        {
            var content = await _service.FetchRecordsAsByteArrayAsync(true);

            var contentType = "application/force-download";
            var fileName = "TransformedFile.csv";

            return File(content, contentType, fileName);
        }
    }
}
