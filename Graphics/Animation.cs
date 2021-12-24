using System;
using System.Numerics;
using Raylib_cs;

namespace RaylibExt
{
    public class Animation
    {
        public string Name { get; private set; }
        
        public Point CellSize { get { return new Point(cellW,cellH); } }
        public Point Cells { get { return new Point(cellsX,cellsY);} }

        private int cellH;
        private int cellW;

        private int cellsX;
        private int cellsY;

        private int offsetX;
        private int offsetY;

        private int CurFrame {get {return curFrame;} set{curFrame = value % cellsX*cellsY;}}

        private int curFrame;

        private Rectangle[] cell_src;
        private Texture2D? texture;

        // playback

        public float Interval {get; set;}
        private float timer;

        public Animation(string name, int cellWidth, int cellHeight, float interval = 0.25f)
        {
            Name = name;
            cellW = cellWidth;
            cellH = cellHeight;
            offsetX = 0;
            offsetY = 0;
            Interval = interval;
        }

        public Animation(string name, int cellWidth,int cellHeight, int startX, int startY, int cellsX, int cellsY, float interval = 0.25f)
		{
            Name = name;
            this.offsetX = startX;
            this.offsetY = startY;
            this.cellsX = cellsX;
            this.cellsY = cellsY;
            cellW = cellWidth;
            cellH = cellHeight;
            Interval = interval;
		}

        public void GenerateCells(Texture2D texture)
		{
            this.texture = texture;
            if (cellsX == 0 && cellsY == 0){
                cellsX = texture.width / cellW;
                cellsY = texture.height / cellH;
            }
            cell_src = new Rectangle[cellsX * cellsY];
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"New animation {Name} has ({cellsX}:{cellsY}) cells");

            // calculate the source rectangles
            for (int y = 0; y < cellsY; y++)
            {
                for (int x = 0; x < cellsX; x++)
                {
                    int i = y * cellsX + x;
                    int xx = x + offsetX;
                    int yy = y + offsetY;
                    cell_src[i] = new Rectangle(xx * cellW, yy * cellH, cellW, cellH);
                }
            }
            Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Generated cells for animation {Name}");
        }

        public void Reset(){
            timer = 0;
        }

        public bool Draw(Vector2 pos)
        {
            if (texture == null)
			{
                Raylib.TraceLog(TraceLogLevel.LOG_ERROR,$"Animation {Name} not yet initialized!");
                return false;
			}
            if (CurFrame < 0 || CurFrame >= cellsX * cellsY)
            {
                return false;
            }
            Rectangle source = cell_src[CurFrame];
            Raylib.DrawTextureRec(texture.Value,source, pos, Color.WHITE);
            timer += Raylib.GetFrameTime();
            if (timer > Interval){
                timer = 0;
                CurFrame++;
            }
            return true;
        }
    }
}
