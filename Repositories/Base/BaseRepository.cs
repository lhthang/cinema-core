using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Base
{
    public class BaseRepository
    {
        protected MyDbContext dbContext;

        protected BaseRepository(MyDbContext context)
        {
            dbContext = context;
        }

        protected void Save()
        {
            var save = dbContext.SaveChanges();
            if (save <= 0)
                throw new Exception("Cannot Save Changes to Database");
        }
    }
}
