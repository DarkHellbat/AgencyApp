﻿using Agency.Models.Filters;
using Agency.Models.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Repository
{
    public class BinaryFileRepository : Repository<BinaryFile, BaseFilter>
    {
        public BinaryFileRepository(ISession session) : base(session)
        {
        }
    }
}
