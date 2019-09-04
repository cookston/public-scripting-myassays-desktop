using JetBrains.Annotations;
using MyAssays.Data.Roslyn;
using MyAssays.Script.TestBed.Core.Mocks;

//this namespaces referenced to MyAssays Desktop engine by default
//any additional namespaces should be also added to Desktop script using <Namespaces> tag

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MyAssays.Data.Layout;
using System.Text;
using MyAssays.Data;
using MyAssays.Data.Pmc;
using MyAssays.Data.Persistence;
using MyAssays.Data.Xml;

namespace MyAssays.Script.TestBed.Core
{
    public class Script
    {
        public ReportIn In { get; }
        public IReportScriptOut Out { get; }

        //path to the test protocol located at MyAssays.Script.TestBed.Core 
        public const string ProtocolFilePath = "Protocols/ELISA (96 well).assay-protocol";
        //Script element Id located at MTA protocol. If exists, it will be replaced with current mock, otherwise new Script report element will be created
        public const string ScriptId = "Script1";
        //Export type for ReportOut project
        public const ReportOutExportType ReportOutType = ReportOutExportType.Xml;

        public Script([NotNull] ReportIn reportIn, [NotNull] IReportScriptOut reportOut)
        {
            In = reportIn ?? throw new ArgumentNullException(nameof(reportIn));
            Out = reportOut ?? throw new ArgumentNullException(nameof(reportOut));
        }

        public void ExecuteScript()
        {
            //Do not change code outside of this method
            //Type your script code code here
            //You can paste code from this method directly to MAD Script report 
            Out.WriteTextElement("Hello world");
        }
    }
}
