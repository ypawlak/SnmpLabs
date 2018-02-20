using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmiParser.Utils
{
    public static class FileUtils
    {
        public static string GetAllFileAsText(string path, string filename)
        {
            string filePath = Path.Combine(path, filename);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filename);
            return File.ReadAllText(filePath);
        }
    }
}
