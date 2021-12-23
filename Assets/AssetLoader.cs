using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RaylibExt
{
	public abstract class AssetLoader<T>
	{
		// a dictionary that stores loaded assets of this type
        public Dictionary<string,T> Assets { get; private set;}

        private string[] filters;

		public AssetLoader(string path, string[] fileExtensions)
		{
            filters = fileExtensions;

            // add a trailing / to the end of the path if there isn't one already
            if (!path.EndsWith('/'))
                path += '/';

            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Loading assets at {path}");
            int chopLength = path.Length;
            Assets = new Dictionary<string, T>();

            // index asset files
            List<string> files = Utils.CrawlDirectory(path);
            foreach (var file in files)
            {
                Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Loading {file}...");

                // generate name for the asset
                // chop of the "assets" portion from the path, change slashes in path to underscores, remove extension
                string name = file.Remove(0, chopLength).Replace('/', '_').Replace('\\', '_');
                name = name.Substring(0, name.LastIndexOf('.'));
                if (Assets.ContainsKey(name))
                {
                    Raylib.TraceLog(TraceLogLevel.LOG_WARNING, $"Asset named {name} already exists!");
                    continue;
                }

                // check if the file's extension matches to our filters
				foreach (var filter in filters)
				{
                    if (file.EndsWith(filter)) {
                        T asset = CreateAssetFromFile(file);
                        Assets.Add(name, asset); // TODO multiple files handling
                        Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Assigned name {name} to asset {file}");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get an asset from this loader, by name.
        /// If it doesn't exist, it returns a placeholder or null if no placeholder is defined.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetAsset(string name)
        {
            if (Assets.ContainsKey(name))
            {
                T item = Assets[name];
                if (item.GetType() == typeof(Sprite))
                {
                    return item;
                }
                return default;
            }
            Raylib.TraceLog(TraceLogLevel.LOG_WARNING, $"Asset {name} doesn't exist!");
            return default;
        }

        /// <summary>
        /// Returns a list containing all the loaded assets
        /// </summary>
        /// <returns></returns>
        public List<T> GetLoadedAssets()
        {
            List<T> assets = new List<T>();
            foreach (var asset in Assets)
            {
                assets.Add(asset.Value);
            }
            return assets;
        }

        /// <summary>
        /// How does the loader deserialize a file?
        /// (See example TextureLoader)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		protected abstract T CreateAssetFromFile(string path);
	}
}
