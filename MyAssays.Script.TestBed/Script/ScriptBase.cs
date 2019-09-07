using System;
using JetBrains.Annotations;
using MyAssays.Data.Roslyn;

namespace MyAssays.Script.TestBed.Core
{
    public partial class Script
    {
        public ReportIn In { get; }
        public IReportScriptOut Out { get; }

        public Script([NotNull] ReportIn reportIn, [NotNull] IReportScriptOut reportOut)
        {
            In = reportIn ?? throw new ArgumentNullException(nameof(reportIn));
            Out = reportOut ?? throw new ArgumentNullException(nameof(reportOut));
        }
    }
}
