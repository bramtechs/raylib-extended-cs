using Raylib_cs;
using RaylibExtendedCS;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RaylibExt
{
    // overengineered widgets for debugging use
    public interface IWidget
    {
        public void Draw();
    }
    public class Button : IWidget
    {
        private Rectangle _region;
        private string _text;

        private Color _colorNormal;
        private Color _colorPressed;
        private Color _colorHover;
        private Color _colorDisabled;
        private bool _enabled;
        private Font _font;
        private int _fontSize;

        public string Text => _text;
        public Rectangle Region => _region;
        public bool Enabled => _enabled;
        public int FontSize => _fontSize;

        public Action? OnClicked;

        public Button(Rectangle region, Color? color = null, string text = "")
        {
            _region = region;
            _text = text;
            _enabled = true;
            _fontSize = 14;
            Color c = color.GetValueOrDefault(Color.LIGHTGRAY);

            _colorNormal = c;
            _colorPressed = Utils.ChangeColorBrightness(c, -20);
            _colorHover = Utils.ChangeColorBrightness(c, 50);
            _colorDisabled = Utils.ChangeColorBrightness(c, -50);

            _font = Raylib.GetFontDefault();
        }
        public bool IsClicked()
        {
            return Utils.IsClickedOn(_region);
        }
        public void Draw()
        {
            Color color = _colorNormal;
            if (_enabled)
            {
                if (Utils.IsHoveringOn(_region))
                {
                    color = _colorHover;
                }
                if (Utils.IsClickedOn(_region))
                {
                    color = _colorPressed;
                    OnClicked?.Invoke();
                }
            }
            else
            {
                color = _colorDisabled;
            }
            Raylib.DrawRectangleRec(_region, color);
            Vector2 size = Raylib.MeasureTextEx(_font, _text, _fontSize, 4);
            Vector2 pos = new Vector2(_region.x + _region.width * 0.5f - size.X * 0.5f, _region.y + _region.height * 0.5f - size.Y * 0.5f);
            Raylib.DrawTextEx(_font, _text, pos, _fontSize, 4, GetTextColor(color));
        }

        private Color GetTextColor(Color bgColor)
        {
            return (bgColor.r < 80 || bgColor.g < 80 || bgColor.b < 80) ? Color.WHITE : Color.BLACK;
        }

    }
    // Handy in situations you don't know how many widgets you need.
    public class WidgetHeap : Heap<IWidget>
    {
        public Button DrawButton(object owner, Rectangle region, Color? color, string text = "")
        {
            Button? button = (Button) GetExisting(owner);
            if (button == null) // the owner doesn't have a button assigned yet
            {
                button = new Button(region, color, text);
                Register(owner, button);
            }
            button.Draw();
            return button;
        }
    }

    public static partial class Utils
    {
        // Only works in global space!
        public static bool IsClickedOn(Rectangle region)
        {
            return IsHoveringOn(region) && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON);
        }
        public static bool IsHoveringOn(Rectangle region)
        {
            return Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), region);
        }

        public static Color ChangeColorBrightness(Color c, int v)
        {
            return ChangeColorBrightness(c, v, v, v, 0);
        }
        public static Color ChangeColorBrightness(Color c, int r, int g, int b, int a = 0)
        {
            int rr = Math.Clamp(c.r + r, 0, 255);
            int gg = Math.Clamp(c.g + g, 0, 255);
            int bb = Math.Clamp(c.b + b, 0, 255);
            int aa = Math.Clamp(c.a + a, 0, 255);
            return new Color(rr, gg, bb, aa);
        }
    }
}
