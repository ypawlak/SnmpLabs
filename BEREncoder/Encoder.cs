using SmiParser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BerEncoding
{
    public class Encoder
    {
        public static readonly Dictionary<SmiEnums.DataTypeBase, byte> TagNumbersByTypes =
            new Dictionary<SmiEnums.DataTypeBase, byte>
        {
            { SmiEnums.DataTypeBase.INTEGER, 2 },
            { SmiEnums.DataTypeBase.BIT_STRING, 3 },
            { SmiEnums.DataTypeBase.OCTET_STRING, 4 },
            { SmiEnums.DataTypeBase.NULL, 5},
            { SmiEnums.DataTypeBase.OBJECT_IDENTIFIER, 6},
            { SmiEnums.DataTypeBase.SEQUENCE, 16}
        };

        public static List<byte> Encode(IDataType dataType, string value)
        {
            var encodedOctets = new List<byte>();

            byte identifierOctet = EncodeIdentifier(dataType);
            encodedOctets.Add(identifierOctet);

            if (dataType.BaseType == SmiEnums.DataTypeBase.INTEGER)
            {
                byte[] valOctets = BitConverter.GetBytes(long.Parse(value));
                byte lengthOctet = (byte)valOctets.Length;
                encodedOctets.Add(lengthOctet);
                encodedOctets.AddRange(valOctets.Reverse().ToArray());
            }
            else if (dataType.BaseType == SmiEnums.DataTypeBase.OCTET_STRING)
            {
                if (value.Length <= 127)
                {
                    byte length = 0;
                    length |= (byte)value.Length;
                    encodedOctets.Add(length);
                }
                else
                {
                    byte kOctets = GetNumberOfOctets(value.Length);
                    encodedOctets.Add(kOctets);
                    var bytes = BitConverter.GetBytes(value.Length);
                    for (var i = 0; i < kOctets; i++)
                        encodedOctets.Add(bytes[sizeof(int) - kOctets + i]);
                }

                for (var i = 0; i < value.Length; i++)
                    encodedOctets.Add((byte)value[i]);
            }

            return encodedOctets;
        }

        private static byte EncodeIdentifier(IDataType dataType)
        {
            byte identifierOctet = 0; //Class 00 - universal
            if (dataType is CustomDataType customDt)
            {
                switch (customDt.Scope)
                {
                    case SmiEnums.DataTypeScope.APPLICATION:
                        identifierOctet |= (1 << 6);
                        break;
                    case SmiEnums.DataTypeScope.CONTEXT_SPECIFIC:
                        identifierOctet |= (1 << 7);
                        break;
                    case SmiEnums.DataTypeScope.PRIVATE:
                        identifierOctet |= (1 << 6);
                        identifierOctet |= (1 << 7);
                        break;
                    default:
                        break;
                }
            }
            //5th bit on means constructed syntax
            if (dataType.BaseType == SmiEnums.DataTypeBase.SEQUENCE)
                identifierOctet |= (1 << 5);

            identifierOctet |= TagNumbersByTypes[dataType.BaseType];

            return identifierOctet;
        }

        private static byte GetNumberOfOctets(int length)
        {
            return (byte)Math.Ceiling((Math.Log(length, 2) + 1) / 8);
        }
    }
}
