using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game2048
{
    public partial class Game
    {
        private readonly Tile[,] map;
        public readonly int Width;
        public readonly int Height;
        private readonly HashSet<Point> emptyPositions = new HashSet<Point>();
        public int Score { get; private set; }

        public Game(int width, int height)
        {
           if (width <= 0 || height <= 0 || width * height < 2)
               throw new ArgumentException("Wrong map parameters");
           map = new Tile[width, height];
           Width = width;
           Height = height;
    
           for (var y = 0; y < Height; y++)
               for (var x = 0; x < Width; x++)
               {
                   map[x, y] = new Tile();
                   emptyPositions.Add(new Point(x, y));
               }
           Transitions.Push(new List<Transition>());
           FillMovePresets();
           AddRandomTile();
           AddRandomTile();
        }

        public Tile this[int x, int y] => map[x, y];
        public Tile this[Point point] => map[point.X, point.Y];
        public bool IsEmpty(Point point) => emptyPositions.Contains(point);

        private bool InBounds(Point point) =>
            point.X < Width && point.Y < Height &&
            point.X >= 0 && point.Y >= 0;
        
        public void AddRandomTile()
        {
            var random = new Random();
            var point = emptyPositions.ElementAt(random.Next(emptyPositions.Count));
            var value = random.NextDouble() < 0.9 ? 2 : 4;
            ChangeTile(point, value);
            Transitions.Peek().Add(new Transition(new Point(-1, -1), point, value, Condition.Appeared));
        }
        
        public void ChangeTile(Point point, int value)
        {
            this[point].ChangeValue(value);
            if (value != 0 && emptyPositions.Contains(point))
                emptyPositions.Remove(point);
            else if (value == 0)
                emptyPositions.Add(point);
        }

        public bool HasEnded()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (TileHasEqualNeighbours(i, j) || this[i, j].Value == 0)
                        return false;
                }
            }

            return true;
        }

        private bool TileHasEqualNeighbours(int x, int y)
        {
            var point = new Point(x, y);
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    var neighbour = new Point(point.X + dx, point.Y + dy);
                    if (!InBounds(neighbour) || neighbour == point || dx != 0 && dy != 0)
                        continue;

                    if (this[neighbour].Value == this[point].Value)
                        return true;
                }
            }
            return false;
        }
        
        public override string ToString()
        {
            var s = new StringBuilder();
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    s.Append(this[j, i].Value.ToString() + ' ');
                }
                s.Append('\n');
            }
            return s.ToString();
        }
    }
}
