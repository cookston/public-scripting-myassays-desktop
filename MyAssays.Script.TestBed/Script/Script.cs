using JetBrains.Annotations;
using MyAssays.Data.Roslyn;
using MyAssays.Script.TestBed.Core.Mocks;

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

namespace MyAssays.Script.TestBed.Core
{
    public partial class Script
    {
        public void ExecuteScript()
        {
            //Do not change code outside of this method
            //Type your script code code here
            //You can paste code from this method directly to MAD Script report element
            Out.WriteTextElement("Hello world");
        }
    }
}
