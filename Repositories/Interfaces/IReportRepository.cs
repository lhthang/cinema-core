using cinema_core.DTOs.ReportDTOs;
using cinema_core.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IReportRepository
    {
        public ICollection<ReportDTO> GetReport(ReportRequest reportRequest);
    }
}
