using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logger.Infra
{
    public class AppSettings
    {
        public bool DisableLogs { get; set; }

        public bool DisableExceptions { get; set; }
    }
}
