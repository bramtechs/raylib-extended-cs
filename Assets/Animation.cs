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

        private Rectangle[] cell_src;
        private Texture2D? texture;

        public Animation(string name, int cellWidth, int cellHeight)
        {
            Name = name;
            cellW = cellWidth;
            cellH = cellHeight;
        }

        public Animation(string name, int startX, int startY, int cellWidth,int cellHeight, int cellsX, int cellsY)
		{

		}

        public void GenerateCells(Texture2D texture)
		{
            this.texture = texture;
            cellsX = texture.width / cellW;
            cellsY = texture.height / cellH;
            
            cell_src = new Rectangle[cellsX * cellsY];
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, $"New animation {Name} has ({cellsX}:{cellsY}) cells");

            // calculate the source rectangles
            for (int y = 0; y < cellsY; y++)
            {
                for (int x = 0; x < cellsX; x++)
                {
                    int i = y * cellsX + x;
                    cell_src[i] = new Rectangle(x * cellW, y * cellH, cellW, cellH);
                }
            }
            Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Generated cells for animation {Name}");
        }

        public bool DrawCell(Vector2 pos, ushort cellID)
        {
            if (texture == null)
			{
                Raylib.TraceLog(TraceLogLevel.LOG_ERROR,$"Animation {Name} not yet initialized!");
                return false;
			}
            if (cellID < 0 || cellID >= cellsX * cellsY)
            {
                return false;
            }
            Rectangle source = cell_src[cellID];
            Raylib.DrawTextureRec(texture.Value,source, pos, Color.WHITE);
            return true;
        }
    }
}
