using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class MibFile
    {
        public ObjectNode Root { get; set; }
        public IEnumerable<IDataType> DataTypes { get; set; }

        //public IDictionary<string>
    }
}
