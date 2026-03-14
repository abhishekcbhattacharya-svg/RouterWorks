using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router.Tasks
{
    public class RouterConfig
    {
        public bool RemoveOld { get; set; } = true;
        public required string Ip { get; set; }
        public required string RouterPassword { get; set; }
    }
}
