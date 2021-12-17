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

        private Texture2D _placeHolder;

        public AssetManager(string path, string placeHolderTexturePath)
        {
            _placeHolder = Raylib.LoadTexture(placeHolderTexturePath);

            // add a trailing / to the end of the path if there isn't one already
            if (!path.EndsWith('/'))
                path += '/';
            
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Loading assets at {path}");
            int chopLength = path.Length;            
            loadedAssets = new Dictionary<string, Asset>();
           
            // index asset files
            List<string> files = Utils.CrawlDirectory(path);
            foreach (var file in files)
            {
                Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Loading {file}...");
                
                // generate name for the asset
                // chop of the "assets" portion from the path, change slashes in path to underscores, remove extension
                string name = file.Remove(0,chopLength).Replace('/', '_').Replace('\\','_');
                name = name.Substring(0,name.LastIndexOf('.'));
                if (loadedAssets.ContainsKey(name))
				{
                    Raylib.TraceLog(TraceLogLevel.LOG_WARNING,$"Asset named {name} already exists!");
				    continue;
                } 
                // determine file type from path
                Asset asset = null;
                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
				{
                    asset = new TextureAsset(file);
				} //else if (file.EndsWith(".wav") || file.EndsWith(".ogg") || file.EndsWith(".flac") || file.EndsWith(".mp3")){
                   // not implemented 
                //}
				else
				{
                    Raylib.TraceLog(TraceLogLevel.LOG_WARNING,$"Unknown file format of file {file}, ignoring...");
				    continue;
                }
                loadedAssets.Add(name,asset); // TODO multiple files handling
                Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Assigned name {name} to asset {file}");
            }


        }
        
        public Texture2D GetTexture(string name)
		{
            if (loadedAssets.ContainsKey(name))
			{
                Asset item = loadedAssets[name];
                if (item.GetType() == typeof(TextureAsset))
				{
                    return ((TextureAsset)item).Texture;
				}
                return _placeHolder; // TODO replace with default texture
            }
            Raylib.TraceLog(TraceLogLevel.LOG_WARNING,$"Texture {name} doesn't exist!");
		    return _placeHolder;
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
