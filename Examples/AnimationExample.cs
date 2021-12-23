using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Numerics;

namespace RaylibExt
{
	public class AnimationExample
	{
        public static int Main()
        {
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
            
            Sprite placeHolder = new Sprite("Assets/Gfx/invalid.png");
            TextureLoader loader = new TextureLoader("Assets/Gfx",placeHolder);
            Sprite sprite = loader.GetAsset("character_robot_sheet");
            sprite.Position = new Vector2(10,10);

            SetTargetFPS(60);

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText("Congrats! You created your first window!", 190, 200, 20, MAROON);
                sprite.Draw();

                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }
}
