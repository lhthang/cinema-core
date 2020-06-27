using cinema_core.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IReportRepository
    {
        public ICollection<ReportDTO> GetReport(int month, int year);
    }
}
