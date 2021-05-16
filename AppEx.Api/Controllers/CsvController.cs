using AppEx.Services.CSV;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppEx.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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

        /// <summary>
        /// Download CSV file which was loaded from waterconnect.sa.gov.au with transformations of: 1). remove column 'Unit_No', and 2). add new column 'Calc'
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/v1/Csv/WaterConnect
        /// </remarks>
        /// <returns>CSV file</returns>
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
