using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BerEncoding
{
    public class Decoder
    {
        public static DecodedObjectMeta DecodeObject(byte[] bytes)
        {
            return DecodeObjects(bytes).Single();
        }
        private static List<DecodedObjectMeta> DecodeObjects(byte[] bytes)
        {
            List<DecodedObjectMeta> decodedList = new List<DecodedObjectMeta>();
            if (bytes.Any())
            {
                DecodedObjectMeta decodedMeta = DecodeIdentifier(bytes);
                DecodeLength(bytes, out var length, out var dataStart);

                decodedMeta.ValueLength = length;
                decodedMeta.ValueBytes = bytes.Skip(dataStart).Take((int)length).ToArray();

                if (decodedMeta.Constructed)
                {
                    decodedMeta.Children = DecodeObjects(
                        bytes
                            .Skip(dataStart)
                            .Take((int)length)
                            .ToArray()
                        );
                    foreach (DecodedObjectMeta child in decodedMeta.Children)
                        child.Parent = decodedMeta;
                }
                else
                {
                    decodedMeta.Children = new List<DecodedObjectMeta>();
                }

                decodedList.Add(decodedMeta);
                byte[] remainingBytes = bytes.Skip(dataStart + (int)length).ToArray();
                decodedList.AddRange(DecodeObjects(remainingBytes));
            }

            return decodedList;
        }

        private static DecodedObjectMeta DecodeIdentifier(byte[] berBytes)
        {
            var decodedMeta = new DecodedObjectMeta();

            var identifierOctet = berBytes[0];
            if ((identifierOctet & 1 << 7) == 0)
            {
                decodedMeta.Class = (identifierOctet & 1 << 6) == 0
                    ? SmiParser.SmiEnums.DataTypeScope.UNIVERSAL
                    : SmiParser.SmiEnums.DataTypeScope.APPLICATION;
            }
            else
            {
                decodedMeta.Class = (identifierOctet & 1 << 6) == 0
                    ? SmiParser.SmiEnums.DataTypeScope.CONTEXT_SPECIFIC
                    : SmiParser.SmiEnums.DataTypeScope.PRIVATE;
            }

            decodedMeta.Constructed = (identifierOctet & 1 << 5) != 0;
            decodedMeta.Tag = identifierOctet & 31;
            return decodedMeta;
        }

        private static void DecodeLength(byte[] berBytes, out long length, out int dataStart)
        {
            //short type
            if (berBytes[1] <= 127)
            {
                length = berBytes[1];
                dataStart = 2;
            }
            //long type
            else
            {
                //k represents count of bytes carrying length
                int k = berBytes[1] & 127;
                length = BitConverter.ToInt64(
                    berBytes
                        .Skip(2)
                        .Take(k)
                        .ToArray(),
                    0);
                dataStart = k + 2;
            }
        }
    }
}
