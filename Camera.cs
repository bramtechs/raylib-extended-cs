using Raylib_cs;
using System.Numerics;


namespace RaylibExt 
{
    public static partial class Utils
    {
        public static void MoveCameraWithKeyboard(ref Camera2D cam, int speed)
        {
            Vector2 offset = Vector2.Zero;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                offset -= Vector2.UnitX * Raylib.GetFrameTime() * speed / cam.zoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                offset += Vector2.UnitX * Raylib.GetFrameTime() * speed / cam.zoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                offset += Vector2.UnitY * Raylib.GetFrameTime() * speed / cam.zoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
                offset -= Vector2.UnitY * Raylib.GetFrameTime() * speed / cam.zoom;
            }
            cam.target += offset;
        }

        public static void ZoomCameraWithKeyboard(ref Camera2D cam, int zoomSpeed)
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_PAGE_DOWN))
            {
                cam.zoom -= Raylib.GetFrameTime() * zoomSpeed;
                if (cam.zoom < 0.1) cam.zoom = 0.1f;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_PAGE_UP))
            {
                cam.zoom += Raylib.GetFrameTime() * zoomSpeed;
                if (cam.zoom > 3) cam.zoom = 3;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_HOME))
            {
                cam.zoom = 1;
            }
        }
    }

}