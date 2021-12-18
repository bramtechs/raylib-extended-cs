using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RaylibExt
{
    public abstract class Asset : IDisposable
    {
        protected string _sourcePath;
        
        public Asset(string sourcePath)
        {
            _sourcePath = sourcePath;
        }
        public abstract void Dispose();
        public override string ToString() => $"{(this.GetType().Name)}       {_sourcePath}";
    }
    public class TextureAsset : Asset 
    {
        private Texture2D _texture;
        
        public Texture2D Texture => _texture;
        
        public TextureAsset(string path) : base(path)
        {
            _texture = Raylib.LoadTexture(path);
        }
        
        public override void Dispose()
        {
            Raylib.UnloadTexture(_texture);
        }
    }
}
