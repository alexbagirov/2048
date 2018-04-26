using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game2048
{
    public class GameMap
    {
        private readonly Tile[,] gameMap;
        public readonly int Width;
        public readonly int Height;
        
        public GameMap(int width, int height)
        {
            gameMap = new Tile[width, height];
            Width = width;
            Height = height;
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    gameMap[x, y] = new Tile();
        }

        public Tile this[int x, int y]
        {
            get { return gameMap[x, y]; }
            set { gameMap[x, y] = value; }
        }

        public Tile this[Point point]
        {
            get { return gameMap[point.X, point.Y]; }
            set { gameMap[point.X, point.Y] = value; }
        }

        public bool InBounds(Point point)
        {
            return point.X < Width && point.Y < Height && point.X >= 0 && point.Y >= 0;
        }
    }
}
