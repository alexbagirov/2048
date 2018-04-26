using System;
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
            get => gameMap[x, y];
            set => gameMap[x, y] = value;
        }

        public Tile this[Point point]
        {
            get => gameMap[point.X, point.Y];
            set => gameMap[point.X, point.Y] = value;
        }

        public void AddTile(Point point, int value) => this[point].Value = value;

        public Point GetEmptyTile()
        {
            var random = new Random();
            while (true)
            {
                var x = random.Next(0, Width);
                var y = random.Next(0, Height);
                if (gameMap[x, y].Value != 0)
                    continue;
                return new Point(x, y);
            }
        }

        public void MoveTile(Point from, Point to)
        {
            this[to].Value += this[from].Value;
            this[from].Value = 0;
        }

        public bool InBounds(Point point) => 
            point.X < Width && point.Y < Height && 
            point.X >= 0 && point.Y >= 0;
    }
}
