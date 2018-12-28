using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Mvc.Models
{
    public class HealthCheckStatus
    {
        public string ServiceName { get; set; }

        public string Status { get; set; }
    }
}
