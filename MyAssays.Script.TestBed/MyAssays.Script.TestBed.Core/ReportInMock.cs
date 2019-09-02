using System;
using JetBrains.Annotations;
using MyAssays.AssayPackageManager;
using MyAssays.Data.Interfaces;
using MyAssays.Data.Roslyn;

namespace MyAssays.Script.TestBed.Core
{
    public class ReportInMock : ReportIn
    {
        public override Version MADVersion => typeof(IAssayPackage).Assembly.GetName().Version;

        public ReportInMock(IProcessorData processorData) : base(processorData) { }

        public void SetValueEvaluator(IEvaluatorFacade evaluatorFacade)
        {
            EvaluatorFacade = evaluatorFacade ?? throw new ArgumentNullException(nameof(evaluatorFacade));
        }

        public void SetUContainerNum(int uContainerNum)
        {
            UContainerNum = uContainerNum;
        }
    }
}