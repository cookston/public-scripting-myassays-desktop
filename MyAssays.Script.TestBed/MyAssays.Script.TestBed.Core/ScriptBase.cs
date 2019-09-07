using System;
using JetBrains.Annotations;
using MyAssays.Data.Roslyn;

namespace MyAssays.Script.TestBed.Core
{
    public class ScriptBase
    {
        public ReportIn In { get; private set; }
        public IReportScriptOut Out { get; private set; }

        internal void Init([NotNull] ReportIn reportIn, [NotNull] IReportScriptOut reportOut)
        {
            In = reportIn ?? throw new ArgumentNullException(nameof(reportIn));
            Out = reportOut ?? throw new ArgumentNullException(nameof(reportOut));
        }
    }
}
