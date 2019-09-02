using System;
using System.Collections.Generic;
using System.Linq;
using MyAssays.Data.Persistence;
using MyAssays.Data.Roslyn;
using MyAssays.Data.Xml;

namespace MyAssays.Script.TestBed.Core
{
    public class ReportOutMock : IReportScriptOut
    {
        public List<string> OutStrings { get; } = new List<string>();
        public string Error { get; private set; }

        public void WriteXml(string xml)
        {
            try
            {
                //because this is mock we don't need add Report Xml element to real report so only id's will be added to output
                var report = MyAssaysObjectSerializer.DeserializeString<Report>(AddRootReportTag(xml));
                if (report?.Items != null)
                {
                    foreach (var baseReportElement in report.Items.OfType<BaseReportElementWithId>())
                    {
                        OutStrings.Add($"Element with id {baseReportElement.Id} successfully added.");
                    }
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public void WriteTextElement(string s)
        {
            OutStrings.Add(s);
        }

        public void LogError(Exception e)
        {
            Error = $"Exception: {e.Message} {e.StackTrace}";
        }

        //todo probably reuse ReportXmlGenerator functionality here
        private string AddRootReportTag(string report)
        {
            //try to add missed root elements
            if (!report.TrimStart().StartsWith("<Report"))
            {
                report = string.Concat("<Report>\r\n", report);
            }

            if (!report.TrimEnd().EndsWith("</Report>"))
            {
                report = string.Concat(report, "\r\n</Report>");
            }

            return report;
        }
    }
}
