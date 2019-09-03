using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAssays.Data.Roslyn;
using MyAssays.Script.TestBed.Core;
using MyAssays.Script.TestBed.Core.Mocks;

namespace MyAssays.Script.TestBed.ReportOut
{
    class Program
    {
        private static Core.TestBed _testBed;
        private static IReportScriptOut Out => _testBed.Out;

        static void Main(string[] args)
        {
            //Do not change any code in current method! Use ExecuteScript method instead
            try
            {
                _testBed = new Core.TestBed(Core.Script.ProtocolFilePath, new Data.Roslyn.ReportOut());
                _testBed.ExecuteReport(Core.Script.ReportOutType, Core.Script.ScriptId);

                if (!string.IsNullOrEmpty(Out.Error))
                {
                    Console.WriteLine("Script execution complete with errors.");
                    Console.WriteLine(Out.Error);
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed execute script: {e.Message}");
                Console.ReadKey();
            }
        }
    }
}
