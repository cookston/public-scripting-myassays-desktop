using System;
using MyAssays.Data.Roslyn;
using MyAssays.Script.TestBed.Core;
using MyAssays.Script.TestBed.Core.Mocks;

namespace MyAssays.Script.TestBed.ConsoleOut
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
                _testBed = new Core.TestBed(Core.Script.ProtocolFilePath, new ReportOutMock());
                _testBed.ExecuteConsole();

                if (!string.IsNullOrEmpty(Out.Error))
                {
                    Console.WriteLine("Script execution complete with errors.");
                    Console.WriteLine(Out.Error);
                }
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
    }
}
