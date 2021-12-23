using System;

namespace RaylibExt
{
	public class TextureLoader : AssetLoader<Sprite>, IDisposable
	{
		public static TextureLoader Instance { get; private set;}

		public TextureLoader(string path, Sprite placeholder = null) : base(path, new string[] { ".png", ".jpg", ".bmp" })
		{
			Instance = this;
		}

		protected override Sprite CreateAssetFromFile(string path)
		{
			return new Sprite(path);
		}

		public void Dispose()
		{
			foreach (var asset in Assets)
			{
				asset.Value.Dispose();
			}
			Instance = null;
		}
	}
}
