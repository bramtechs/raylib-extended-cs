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

            Sprite sprite = loader.GetAsset("character_robot_sheet").AddAnimations(new Animation[]{
                new Animation("idleing",96,128,0,0,1,1),
                new Animation("running",96,128,6,2,3,1),
                new Animation("climbing",96,128,5,0,2,1),
                new Animation("jumping",96,128,0,0,3,1),
                });
            sprite.Position = new Vector2(350,50);

            SetTargetFPS(60);

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                BeginDrawing();
                ClearBackground(RAYWHITE);
                
                string curAnimation = "idleing";
                if (Raylib.IsKeyDown(KeyboardKey.KEY_D)){
                    curAnimation = "running";
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_A)){
                    curAnimation = "running";
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE)){
                    curAnimation = "jumping";
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_W)){
                    curAnimation = "climbing";
                }


                DrawText($"You should see a robot {curAnimation}!", 190, 200, 20, MAROON);
                sprite.Draw(curAnimation);

                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }
}
