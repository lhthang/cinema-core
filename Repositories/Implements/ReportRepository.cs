using cinema_core.DTOs.ReportDTOs;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ReportRepository : BaseRepository, IReportRepository
    {

        public ReportRepository(MyDbContext context) : base(context)
        {
        }
        public ICollection<ReportDTO> GetReport(int month, int year)
        {
            throw new NotImplementedException();
        }
    }
}
