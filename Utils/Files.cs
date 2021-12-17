using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using System.IO;

namespace RaylibExt 
{
    public static partial class Utils
    {
        public static List<string> CrawlDirectory(string path)
        {
            List<string> paths = new List<string>();
            // get deeper directories in directory
            foreach (string subPath in Directory.GetDirectories(path))
            {
                List<string> subPaths = CrawlDirectory(subPath);
                paths.AddRange(subPaths);
                //Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, subPath);
            }
            // get files in directory
            paths.AddRange(Directory.GetFiles(path));
            return paths;
        }
        
    }
}