﻿using System;
using System.Collections.Generic;
using System.Text;
using static SmiParser.Model.SmiEnums;
namespace SmiParser.Model
{
    public interface IDataType
    {
        string Name { get; }
        DataTypeBase BaseType { get; }

        string RestrictionsDescription { get; }
        bool Validate(string value);
    }
}
