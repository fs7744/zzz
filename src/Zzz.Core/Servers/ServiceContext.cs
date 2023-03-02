using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzz.Abstractions.Servers;

namespace Zzz.Core.Servers
{
    internal class ServiceContext
    {
        public ZzzTrace Log { get; set; } = default!;

        public PipeScheduler Scheduler { get; set; } = default!;

        public ISystemClock SystemClock { get; set; } = default!;

        public ConnectionManager ConnectionManager { get; set; } = default!;

        public Heartbeat Heartbeat { get; set; } = default!;

        public ServerOptions ServerOptions { get; set; } = default!;

        public DiagnosticSource? DiagnosticSource { get; set; }
    }
}
