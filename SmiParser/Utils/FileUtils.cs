using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SmiParser.Utils
{
    public static class FileUtils
    {
        public static string GetAllFileAsText(string innerPath, string filename)
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
