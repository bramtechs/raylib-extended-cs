using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;

namespace RaylibExt
{
	public class Sprite : IDisposable
	{
		public Vector2 Position { get; set; }

		public Texture2D Texture { get; private set; }
		
		private Dictionary<string, Animation> animations;

		private string prevAnimation;

		public Sprite(string texturePath, Animation[] animations = null)
		{
			Texture = Raylib.LoadTexture(texturePath);

			this.animations = new Dictionary<string, Animation>();
			if (animations != null)
			{
				foreach (var anim in animations)
				{
					this.animations.Add(anim.Name, anim);
				}
			}
			AddAnimation(new Animation("none",Texture.width,Texture.height));
		}

		public Sprite AddAnimation(Animation animation)
		{
			this.animations.Add(animation.Name, animation);
			animation.GenerateCells(Texture);
			return this;
		}
		public Sprite AddAnimations(Animation[] animations){
			foreach (Animation anim in animations){
				AddAnimation(anim);
			}
			return this;
		}

		public void Draw(string anim = "none")
		{
			// Resets the previous animation when parameter changed
			if (prevAnimation == null){
				prevAnimation = anim;
			}else if (prevAnimation != anim){
				animations[prevAnimation].Reset();
				prevAnimation = anim;
			}

			if (animations.ContainsKey(anim))
			{
				animations[anim].Draw(Position);
			}
		}

		public Animation GetAnimation(string name)
		{
			if (this.animations.ContainsKey(name))
			{
				return this.animations[name];
			}
			Raylib.TraceLog(TraceLogLevel.LOG_WARNING, $"Animation {name} doesn't exist!");
			return null;
		}

		public void Dispose()
		{
			Raylib.UnloadTexture(Texture);
		}
	}
}
