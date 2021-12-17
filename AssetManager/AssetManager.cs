using System;
using Raylib_cs;
using System.Collections.Generic;
using RaylibExt;

namespace RaylibExt
{
    public class AssetManager : IDisposable
    {
        // a huge dictionary that stores all of your assets
        private Dictionary<string, Asset> loadedAssets;
        public AssetManager(string path)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Loading assets at {path}");
            loadedAssets = new Dictionary<string, Asset>();
            // index asset files
            List<string> files = Utils.CrawlDirectory(path);
            foreach (var file in files)
            {
                Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Loading {file}...");
            }
        }

        public void Dispose()
        {
            foreach (var item in loadedAssets)
            {
                item.Value.Dispose();
            }
        }
    }
}
