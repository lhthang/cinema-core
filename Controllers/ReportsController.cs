using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        private IReportRepository reportRepository;
        public ReportsController(IReportRepository repository)
        {
            reportRepository = repository;
        }

        // POST: api/reports
        [HttpPost]
        [Authorize(Roles = Authorize.Admin+","+Authorize.Staff)]
        public IActionResult Post([FromBody] ReportRequest request)
        {

            try
            {
                var report = reportRepository.GetReport(request);
                return Ok(report);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}