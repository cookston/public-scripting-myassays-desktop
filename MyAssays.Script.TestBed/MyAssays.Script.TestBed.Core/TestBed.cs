using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Moq;
using MyAssays.Analysis.Processor;
using MyAssays.AnalysisPlugins;
using MyAssays.AssayPackageManager;
using MyAssays.Common;
using MyAssays.Data;
using MyAssays.Data.Interfaces;
using MyAssays.Data.Roslyn;
using MyAssays.Data.Xml;
using MyAssays.MatrixTransform.Analysis;
using MyAssays.MatrixTransform.Analysis.Generators;
using MyAssays.ReportXmlConversion;
using MyAssays.Script.TestBed.Core.Mocks;
using MyAssays.Scripting;
using FilePathHelper = MyAssays.Common.FilePathHelper;

namespace MyAssays.Script.TestBed.Core
{
    public class TestBed
    {
        public ReportInMock In { get; }
        public IReportScriptOut Out { get; }

        private readonly IProcessorData _data;
        private static Core.Script _script;
        const string DefaultScriptId = "Script1";

        static string CheckFileExistsInCurrentFolderOrScriptsFolder(string path)
        {
            if (!File.Exists(path))
            {
                var scriptFilePath = $@"..\..\..\Script\{path}";

                if (File.Exists(scriptFilePath))
                {
                    path = scriptFilePath;
                }
                else
                {
                    throw new FileNotFoundException($"File not found at {path}");
                }
            }

            return path;
        }

        public TestBed(string protocolFilePath, [NotNull] IReportScriptOut reportScriptOut)
        {
            Out = reportScriptOut ?? throw new ArgumentNullException(nameof(reportScriptOut));

            protocolFilePath = CheckFileExistsInCurrentFolderOrScriptsFolder(protocolFilePath);

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

            In = new ReportInMock(_data);
            _script = new Script();
            ((ScriptBase) _script).Init(In,Out);
        }

        private void SetupScript(ReportScriptGeneratorParameters parameters)
        {
            In.Report.ContainerNum = parameters.ZContainerNum;
            In.SetUContainerNum(parameters.ZContainerNum + 1);
            In.SetValueEvaluator(parameters.EvaluatorFacade);
            _script.ExecuteScript();
        }

        public void ExecuteConsole()
        {
            Execute(null, DefaultScriptId);
        }

        public void ExecuteReport(ReportOutExportType outExportType, string reportScriptId)
        {
            Execute(outExportType, reportScriptId);
        }

        private void Execute(ReportOutExportType? outExportType, string reportScriptId)
        {
            var script = InitializeScript(outExportType, reportScriptId);
            UpdateOrAddReportElement(outExportType, script);

            var processor = new MatrixTransformAnalysisProcessor(_data);
            var tempPath = FilePathHelper.Instance.GenerateTemporaryFilePath(
                FileConstants.AssayReportFileExtension,
                "Report");
            processor.Process(tempPath);
            ProcessOutputFile(processor, outExportType);
        }

        private MatrixTransformAnalysisReportScript InitializeScript(ReportOutExportType? outExportType, string reportScriptId)
        {
            var scriptFilePath = CheckFileExistsInCurrentFolderOrScriptsFolder("Script.cs");

            var scriptClassText = File.ReadAllText(scriptFilePath);

            var classCode = RoslynHelpers.GetMethodText(scriptClassText, nameof(Script.ExecuteScript));

            var script = outExportType.HasValue ? new MatrixTransformAnalysisReportScript() : new ReportScriptMock(SetupScript);
            script.Id = reportScriptId ?? DefaultScriptId;
            script.Code = classCode;

            return script;
        }

        private void UpdateOrAddReportElement(ReportOutExportType? outExportType, MatrixTransformAnalysisReportScript script)
        {
            var elementAdded = false;
            if (outExportType.HasValue)
            {
                var existingReportScript = _data.MatrixTransformAnalysis.Report.Items.OfType<MatrixTransformAnalysisReportScript>().FirstOrDefault(i => string.Equals(i.Id, script.Id, StringComparison.OrdinalIgnoreCase));
                if (existingReportScript != null)
                {
                    var items = _data.MatrixTransformAnalysis.Report.Items.ToList();
                    var index = items.IndexOf(existingReportScript);
                    items.RemoveAt(index);
                    items.Insert(index, script);
                    _data.MatrixTransformAnalysis.Report.Items = items.ToArray();
                    elementAdded = true;
                }
            }

            if (!elementAdded)
            {
                var items = _data.MatrixTransformAnalysis.Report.Items.ToList();
                items.Add(script);
                _data.MatrixTransformAnalysis.Report.Items = items.ToArray();
            }
        }

        private void ProcessOutputFile(MatrixTransformAnalysisProcessor analysisProcessor, ReportOutExportType? outExportType)
        {
            switch (outExportType)
            {
                case ReportOutExportType.Xml:
                    TryOpenFile(analysisProcessor.OutputReportFilePath);
                    break;
                case ReportOutExportType.Xlsx:
                    var tempReportPath = Path.Combine(Path.GetDirectoryName(analysisProcessor.OutputReportFilePath), "report.xlsx");
                    XmlConverter.Convert(analysisProcessor.OutputReportFilePath, tempReportPath, FileSaveFormat.Xlsx);
                    TryOpenFile(tempReportPath);
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(outExportType), outExportType, null);
            }
        }
        
        private void TryOpenFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Exported file '{filePath}' not found.");
            
            //current copy required to ensure that out file will exists after app closed
            var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
            File.Copy(filePath, tempFilePath, true);

            Process.Start(tempFilePath);
        }
    }
}
