using System;
using MyAssays.Data.Roslyn;
using MyAssays.Script.TestBed.Core;

namespace MyAssays.Script.TestBed
{
    partial class Program
    {
        private static Core.TestBed _testBed;
        private static ReportOutMock Out => _testBed.Out;
        private static ReportInMock In => _testBed.In;

        static void Main(string[] args)
        {
            //Do not change any code in current method! Use ExecuteScript method instead
            try
            {
                _testBed = new Core.TestBed(ProtocolFilePath);
                _testBed.Execute(SetupScript);

                if (!string.IsNullOrEmpty(Out.Error))
                    Console.WriteLine(Out.Error);
                else
                    foreach (var outData in Out.OutStrings)
                    {
                        Console.WriteLine(outData);
                    }

                Console.WriteLine("Script execution complete. Press any key to exit.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed execute script: {e.Message}");
            }

            Console.ReadKey();
        }

        private static void SetupScript(ReportScriptGeneratorParameters parameters)
        {
            In.Report.ContainerNum = parameters.ZContainerNum;
            In.SetUContainerNum(parameters.ZContainerNum + 1);
            In.SetValueEvaluator(parameters.EvaluatorFacade);
            ExecuteScript();
        }
    }
}
