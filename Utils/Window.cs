using System;
using System.Numerics;
using Raylib_cs;

namespace RaylibExt 
{
    public static partial class Utils
    {
        public static Vector2 _size = new Vector2(-1, 1);

        private static float _scale;
        private static int _screenWidth;
        private static int _screenHeight;
        private static uint _curFrame;

        // Clamp Vector2 value with min and max and return a new vector2
        // NOTE: Required for virtual mouse, to clamp inside virtual game size
        public static Vector2 ClampValue(Vector2 value, Vector2 min, Vector2 max)
        {
            Vector2 result = value;
            result.X = (result.X > max.X) ? max.X : result.X;
            result.X = (result.X < min.X) ? min.X : result.X;
            result.Y = (result.Y > max.Y) ? max.Y : result.Y;
            result.Y = (result.Y < min.Y) ? min.Y : result.Y;
            return result;
        }

        public static void AutoScale(int width, int height)
        {
            // Determine a scale for the window that's fits the user's screen
            int curMon = Raylib.GetCurrentMonitor();
            float monSize = Raylib.GetMonitorHeight(curMon);

            int scale = (int)(Math.Max(1.5, (int)monSize / height) - 0.5f);

            Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Current monitor size height is {monSize}");
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Setting the window scale to {scale} according to monitor {curMon}");
            Utils.ScaleWindow(scale);
            Utils.CenterWindow(width * scale, height * scale);
        }

        // Enables letterboxing in your window
        public static void InitLetterbox(int gameWidth, int gameHeight)
        {
            _size = new Vector2(gameWidth, gameHeight);
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Letterbox mode initialized {_size}");
        }

        // Gets the windows scale while letterboxing
        public static float GetWindowScale()
        {
            return _scale;
        }

        // Prevents a weird bug(?)
        public static float PrepareWindowScale()
        {
            int w = _screenWidth = Raylib.GetScreenWidth();
            int h = _screenHeight = Raylib.GetScreenHeight();
            float scale = _scale = Math.Min(w / _size.X, h / _size.Y);
            return scale;
        }

        // Get the virtual mouse position when the window and game resolutions don't match. (letterbox)
        public static Vector2 GetVirtualMousePosition()
        {

            if (_size.X == -1)
            {
                Raylib.TraceLog(TraceLogLevel.LOG_ERROR, "Letterbox mode not initialized!");
                return Vector2.Zero;
            }

            float scale = GetWindowScale();
            Vector2 mouse = Raylib.GetMousePosition();
            Vector2 virtualMouse;
            virtualMouse.X = (mouse.X - (_screenWidth - (_size.X * scale)) * 0.5f) / scale;
            virtualMouse.Y = (mouse.Y - (_screenHeight - (_size.Y * scale)) * 0.5f) / scale;
            virtualMouse = ClampValue(virtualMouse, Vector2.Zero, _size);
            return virtualMouse;
        }
        public static Vector2 GetScreenToWorld2D(Vector2 screenPos, Camera2D cam)
        {
            Vector2 worldPos = Raylib.GetScreenToWorld2D(screenPos, cam);

            float scaleX = _screenWidth / _size.X;
            float scaleY = _screenHeight / _size.Y;

            // TODO finish! (on windows????)

            return worldPos;
        }
        // Draw render texture to screen, properly scaled
        public static void DrawLetterboxGame(RenderTexture2D target)
        {
            float scale = GetWindowScale();
            Raylib.DrawTexturePro(target.texture, new Rectangle(0.0f, 0.0f, target.texture.width, -target.texture.height),
        new Rectangle(
                (Raylib.GetScreenWidth() - (_size.X * scale)) * 0.5f, (Raylib.GetScreenHeight() - (_size.Y * scale)) * 0.5f,
            _size.X * scale, _size.Y * scale
    ), Vector2.Zero, 0.0f, Color.WHITE);
            _curFrame++;
        }

        // Multiply the dimensions of the window
        public static void ScaleWindow(float scale)
        {
            int width = (int)(Raylib.GetScreenWidth() * scale);
            int height = (int)(Raylib.GetScreenHeight() * scale);
            Raylib.SetWindowSize(width, height);
        }

        // Try to center the window on the correct monitor when having one or two.
        public static void CenterWindow()
        {
            CenterWindow(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }

        // Try to center the window on the correct monitor when having one or two.
        public static void CenterWindow(int winWidth, int winHeight)
        {
            // Center the window (if there are two)
            int curMonitor = Raylib.GetCurrentMonitor();
            int x = Raylib.GetMonitorWidth(curMonitor) / 2;
            int y = Raylib.GetMonitorHeight(curMonitor) / 2;

            //offset
            x -= winWidth / 2;
            y -= winHeight / 2;

            // shift the window to the other monitor
            int monCount = Raylib.GetMonitorCount();
            if (monCount == 2)
            {
                int otherMonitor = curMonitor == 0 ? 1 : 0;
                Vector2 curMonPos = Raylib.GetMonitorPosition(curMonitor);
                Vector2 otherMonPos = Raylib.GetMonitorPosition(otherMonitor);

                Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"{curMonitor}: main monitor at position {curMonPos}");
                Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"{otherMonitor}: other monitor at position {otherMonPos}");

                if (curMonPos.X < otherMonPos.X)
                    x += Raylib.GetMonitorWidth(otherMonitor);

            }
            Raylib.SetWindowPosition(x, y);
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"Recentered window at {x}:{y} ({monCount} monitors)");
        }

        // Get's how many frames have been drawn
        public static uint GetFrame() => _curFrame;
    }
}