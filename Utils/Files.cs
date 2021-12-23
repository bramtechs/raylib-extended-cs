using System.Collections.Generic;
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
            if (!Directory.Exists(path))
			{
                Raylib.TraceLog(TraceLogLevel.LOG_WARNING,$"Folder {path} doesn't exist, can't crawl");
                return paths;
			}
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
        
        public static string ReadFileInDirectory(string path)
		{
			foreach (var file in Directory.GetFiles(path))
			{
                if (file.Contains(path))
				{
                    string content = File.ReadAllText(file);
                    return content;
				}
			}
            return null;
		}
    }
}