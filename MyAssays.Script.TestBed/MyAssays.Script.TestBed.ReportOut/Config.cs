using MyAssays.Script.TestBed.Core.Mocks;

namespace MyAssays.Script.TestBed.ReportOut
{
    internal class Config
    {
        //path to the test protocol located at MyAssays.Script.TestBed.Core 
        public const string ProtocolFilePath = "Protocols/ELISA (96 well).assay-protocol";

        //Script element Id located at MTA protocol. If exists, it will be replaced with current mock, otherwise new Script report element will be created
        public const string ScriptId = "Script1";

        //Export type for ReportOut project
        public const ReportOutExportType ReportOutType = ReportOutExportType.Xml;
    }
}
