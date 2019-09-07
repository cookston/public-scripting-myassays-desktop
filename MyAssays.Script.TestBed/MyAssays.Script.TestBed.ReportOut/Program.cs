using System;
using MyAssays.Data.Roslyn;

namespace MyAssays.Script.TestBed.ReportOut
{
    class Program
    {
        private static Core.TestBed _testBed;
        private static IReportScriptOut Out => _testBed.Out;

        static void Main(string[] args)
        {
            //Do not change any code in current method! Use ExecuteScript method in Script.cs instead
            try
            {
                _testBed = new Core.TestBed(Config.ProtocolFilePath, new Data.Roslyn.ReportOut());
                _testBed.ExecuteReport(Config.ReportOutType, Config.ScriptId);

                if (!string.IsNullOrEmpty(Out.Error))
                {
                    Console.WriteLine("Script execution completed with errors.");
                    Console.WriteLine(Out.Error);
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to execute script: {e.Message}");
                Console.ReadKey();
            }
        }
    }
}
