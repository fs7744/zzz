using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzz.Core.Servers
{
    internal interface IHeartbeatHandler
    {
        void OnHeartbeat(DateTimeOffset now);
    }
}
