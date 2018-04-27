using System;
using System.Drawing;
using System.Text;

namespace Game2048
{
    public class GameMap
    {
        private readonly Tile[,] gameMap;
        public readonly int Width;
        public readonly int Height;
        
        public GameMap(int width, int height)
        {
            if (width <= 0 || height <= 0 || width * height < 2)
                throw new ArgumentException("Wrong map parameters");
            gameMap = new Tile[width, height];
            Width = width;
            Height = height;
            
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    gameMap[x, y] = new Tile();
        }

        public Tile this[int x, int y] => gameMap[x, y];

        public Tile this[Point point] => gameMap[point.X, point.Y];

        public void AddTile(Point point, int value) => this[point].ChangeValue(value);

        public Point GetEmptyTilePosition()
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
            this[to].AddValue(this[from].Value);
            this[from].ChangeValue(0);
        }

        public bool InBounds(Point point) => 
            point.X < Width && point.Y < Height && 
            point.X >= 0 && point.Y >= 0;

        public override string ToString()
        {
            var s = new StringBuilder();
            
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    s.Append(gameMap[i, j].Value.ToString() + ' ');
                }

                s.Append('\n');
            }

            return s.ToString();
        }
    }
}
