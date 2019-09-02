//this namespaces referenced to MyAssays Desktop engine by default
//any additional namespaces should be also added to Desktop script using <Namespaces> tag
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MyAssays.Data.Layout;
using System.Text;
using MyAssays.Data;
using MyAssays.Data.Pmc;
using MyAssays.Data.Persistence;
using MyAssays.Data.Xml;

namespace MyAssays.Script.TestBed
{
    partial class Program
    {
        //path to the test protocol located at MyAssays.Script.TestBed.Core 
        private const string ProtocolFilePath = "Protocols/ELISA (96 well).assay-protocol";

        private static void ExecuteScript()
        {
            //Do not change code outside of this method
            //Type your script code code here
            //You can paste code from this method directly to MAD Script report 
            Out.WriteTextElement("Hello world");
        }
    }
}
