using ANSIConsole;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MCFabricDependencyViewer
{
    public record B_and_F
    {
        public B_and_F(List<ANSIString> body)
        {
            this.Body   = body;
            this.Footer = new();
        }
        public List<ANSIString> Body { get; init; }
        public List<ANSIString> Footer { get; set; }
    }
}