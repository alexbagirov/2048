using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game2048
{
    public class Game
    {
        public readonly GameMap Map;
        public int Score { get; private set; }
        
        public Game(int width, int height)
        {
            Map = new GameMap(width, height);
            
            FillMovePresets();
            AddRandomTile();
            AddRandomTile();
        }

        private readonly Dictionary<Direction, MovementPresets> presets = 
            new Dictionary<Direction, MovementPresets>();

        private void FillMovePresets()
        {
            presets[Direction.Up] = new MovementPresets(new Point(0, -1), 
                Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height));
            presets[Direction.Left] = new MovementPresets(new Point(-1, 0), 
                Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height));
            presets[Direction.Down] = new MovementPresets(new Point(0, 1), 
                Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height).Reverse());
            presets[Direction.Right] = new MovementPresets(new Point(1, 0), 
                Enumerable.Range(0, Map.Width).Reverse(), Enumerable.Range(0, Map.Height));
        }

        private bool TrySimpleMove(Point curPos, Point vector, HashSet<Tile> mergedTiles)
        {
            var moved = false;
            while (true)
            {
                var nextPos = new Point(curPos.X + vector.X, curPos.Y + vector.Y);
                if ((!Map.InBounds(nextPos) || Map[nextPos].Value != 0) && 
                    (Map[nextPos].Value != Map[curPos].Value || mergedTiles.Contains(Map[nextPos])))
                    break;
                var curValue = Map[curPos].Value;
                var nextValue = Map[nextPos].Value;
                Map.MoveTile(curPos, nextPos);
                curPos = nextPos;
                moved = true;
                if (nextValue == curValue)
                {
                    mergedTiles.Add(Map[curPos]);
                    Score += Map[curPos].Value;
                    break;
                }
            }
            return moved;
        }

        public bool TryMove(Direction direction)
        {
            var moved = false;
            var presets = this.presets[direction];
            var mergedTiles = new HashSet<Tile>();
            foreach (var y in presets.YRange)
                foreach (var x in presets.XRange)
                {
                    if (Map[x, y].Value == 0) 
                        continue;
                    var curPos = new Point(x, y);
                    moved = TrySimpleMove(curPos, presets.Vector, mergedTiles);
                }
            return moved;
        }

        public void Print()
        {
            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                    Console.Write(Map[x, y].Value.ToString().PadRight(4));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void AddRandomTile()
        {
            var random = new Random();
            var point = Map.EmptyPositions.ElementAt(random.Next(Map.EmptyPositions.Count));
            Map.AddTile(point, random.NextDouble() < 0.9 ? 2 : 4);
        }

        public bool HasEnded()
        {
            for (var i = 0; i < Map.Width; i++)
            {
                for (var j = 0; j < Map.Height; j++)
                {
                    if (TileHasEqualNeighbours(i, j) || Map[i, j].Value == 0)
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
                    if (!Map.InBounds(neighbour) || neighbour == point || dx != 0 && dy != 0)
                        continue;

                    if (Map[neighbour].Value == Map[point].Value)
                        return true;
                }
            }

            return false;
        }
    }
}
