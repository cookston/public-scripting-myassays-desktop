using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using MyAssays.Analysis.Processor;
using MyAssays.AnalysisPlugins;
using MyAssays.AssayPackageManager;
using MyAssays.Common;
using MyAssays.Data;
using MyAssays.Data.Interfaces;
using MyAssays.Data.Roslyn;
using MyAssays.MatrixTransform.Analysis;
using MyAssays.MatrixTransform.Analysis.Generators;
using FilePathHelper = MyAssays.Common.FilePathHelper;

namespace MyAssays.Script.TestBed.Core
{
    public class TestBed
    {
        public ReportInMock In { get; }
        public ReportOutMock Out { get; }

        private readonly IProcessorData _data;

        public TestBed(string protocolFilePath)
        {
            var protocol = (IAssayPackageProtocol)AssayPackageManager.AssayPackageManager.UnPack(protocolFilePath);
            if (string.IsNullOrEmpty(protocol.AssayRunDetailsFilePath) || !File.Exists(protocol.AssayRunDetailsFilePath))
                throw new Exception($"The protocol {Path.GetFullPath(protocolFilePath)} is not ready for analysis. Open it in MyAssays Analysis and save, then try again.");
            
            var pluginsInputStorage = new PluginsInputStorage();
            pluginsInputStorage.StoreAssayXml(protocol.AssayXml.OuterXml);
            pluginsInputStorage.LoadRunDetails(protocol.AssayRunDetailsFilePath);
            pluginsInputStorage.InitialiseTransformInputs();

            var analysisLoggerMock = new Mock<IAnalysisLogger>();
            _data = AnalysisProcessorData.Load(
                pluginsInputStorage.ConfigFilePath,
                pluginsInputStorage.RunDetailsFilePath,
                pluginsInputStorage.PmcFilePath,
                pluginsInputStorage.FieldDataFilePath,
                pluginsInputStorage.AssayFilePath,
                analysisLoggerMock.Object);

            Out = new ReportOutMock();
            In = new ReportInMock(_data);
        }

        public void Execute(Action<ReportScriptGeneratorParameters> action)
        {
            var script = new ReportScriptMock(action)
            {
                Id = "ScriptMock1"
            };

            var items = _data.MatrixTransformAnalysis.Report.Items.ToList();
            items.Add(script);
            _data.MatrixTransformAnalysis.Report.Items = items.ToArray();

            var processor = new MatrixTransformAnalysisProcessor(_data);
            var tempPath = FilePathHelper.Instance.GenerateTemporaryFilePath(
                FileConstants.AssayReportFileExtension,
                "Report");
            processor.Process(tempPath);
        }
    }
}
