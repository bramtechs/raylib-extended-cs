using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RaylibExt
{
    public abstract class Asset : IDisposable
    {
        protected string sourcePath;
        public Asset(string sourcePath)
        {
            this.sourcePath = sourcePath;
        }
        public abstract string GenerateName(string path);
        public abstract void Dispose();
    }
    public class TextureAsset : Asset 
    {
        private Texture2D texture;
        public TextureAsset(string path) : base(path)
        {
            texture = Raylib.LoadTexture(path);
        }
        public override string GenerateName(string path)
        {
            // change slashes to underscores
            string name = path.Replace('/', '_');
            Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Assigning name {name} to asset {path}");
            return name;
        }
        public override void Dispose()
        {
            Raylib.UnloadTexture(texture);
        }
    }
}
