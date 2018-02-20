using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class ImportInfo
    {
        public string Filename { get; set; }
        public IEnumerable<string> Terms { get; set; }
    }
}
