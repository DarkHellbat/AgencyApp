﻿using Agency.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Filters
{
    public class JobseekersFilter : BaseFilter
    {
        public Experience experience { get; set; }
    }
}
