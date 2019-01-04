using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SmiParser.Utils
{
    public static class FileUtils
    {
        /// <summary>
        /// Reads all text from given file existing in build output directory
        /// </summary>
        /// <param name="innerPath">Inner path in output directory (without filename)</param>
        /// <param name="filename"></param>
        /// <returns>Whole file read to string</returns>
        public static string ReadFileFromOutputDirectory(string innerPath, string filename)
        {
            string innerFilePath = Path.Combine(innerPath, filename);
            string filePath = string.Empty;
            filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), innerFilePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filename);
            return File.ReadAllText(filePath);
        }
    }
}
