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
		public string CurrentAnimation
		{
			get {return currentAnim;}
			set
			{
				if (animations.ContainsKey(value))
				{
					CurrentAnimation = value;
				}
				else
				{
					Raylib.TraceLog(TraceLogLevel.LOG_WARNING, $"No animation named {value} exists!");
				}
			}
		}
		
		private Dictionary<string, Animation> animations;
		private string currentAnim;

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
			currentAnim = "none";
		}

		public void AddAnimation(Animation animation)
		{
			this.animations.Add(animation.Name, animation);
			animation.GenerateCells(Texture);
		}

		public void Draw()
		{
			if (animations.ContainsKey(currentAnim))
			{
				animations[currentAnim].DrawCell(Position, 0);
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
