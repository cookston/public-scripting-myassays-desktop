using System;
using MyAssays.Data.Roslyn;
using MyAssays.Data.Xml;

namespace MyAssays.Script.TestBed.Core
{
    public class ReportScriptMock : MatrixTransformAnalysisReportScript
    {
        private readonly Action<ReportScriptGeneratorParameters> _mockAction;

        public ReportScriptMock(Action<ReportScriptGeneratorParameters> mockAction)
        {
            _mockAction = mockAction;
        }

        public override bool GenerateFromOuterScript(ReportScriptGeneratorParameters parameters)
        {
            _mockAction.Invoke(parameters);
            return true;
        }
    }
}
