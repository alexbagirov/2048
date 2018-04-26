using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace Game2048
{
    public class Game
    {
        public GameMap Map;
        public bool IsRunning = true;
        public bool Moved;
        public Game(int width, int height)
        {
            Map = new GameMap(width, height);
            FillMovePresets();
            AddTile();
            AddTile();
        }

        public Dictionary<Direction, MovementPresets> Presets = new Dictionary<Direction, MovementPresets>();

        public void FillMovePresets()
        {
            Presets[Direction.Up] = new MovementPresets(new Point(0, -1), Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height));
            Presets[Direction.Left] = new MovementPresets(new Point(-1, 0), Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height));
            Presets[Direction.Down] = new MovementPresets(new Point(0, 1), Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height).Reverse());
            Presets[Direction.Right] = new MovementPresets(new Point(1, 0), Enumerable.Range(0, Map.Width).Reverse(), Enumerable.Range(0, Map.Height));
        }

        public bool TryMove(string movement)
        {
            var moved = false;
            MovementPresets presets;
            if (movement == "W")
                presets = Presets[Direction.Up];
            else if (movement == "A")
                presets = Presets[Direction.Left];
            else if (movement == "S")
                presets = Presets[Direction.Down];
            else
                presets = Presets[Direction.Right];
            var vector = presets.Vector;
            var xRange = presets.XRange;
            var yRange = presets.YRange;
            var mergedTiles = new HashSet<Tile>();
            foreach (var y in yRange)
            {
                foreach (var x in xRange)
                {
                    if (Map[x, y].Value != 0)
                    {
                        var curPos = new Point(x, y);
                        while (true)
                        {
                            var nextPos = new Point(curPos.X + vector.X, curPos.Y + vector.Y);
                            if (!Map.InBounds(nextPos))
                                break;
                            if (Map[nextPos].Value != 0)
                            {
                                if (Map[nextPos].Value == Map[curPos].Value && !mergedTiles.Contains(Map[nextPos]))
                                {
                                    Map[nextPos].Value += Map[curPos].Value;
                                    Map[curPos].Value = 0;
                                    curPos = nextPos;
                                    mergedTiles.Add(Map[curPos]);
                                    moved = true;
                                }
                                break;
                            }
                            else
                            {
                                Map[nextPos].Value += Map[curPos].Value;
                                Map[curPos].Value = 0;
                                curPos = nextPos;
                                moved = true;
                            }
                        }
                    }
                }
            }
            return moved;
        }

        public void Print()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < Map.Height; y++)
            {
                for (var x = 0; x < Map.Width; x++)
                    Console.Write(Map[x, y].Value.ToString().PadRight(4));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void AddTile()
        {
            var random = new Random();
            while (true)
            {
                var x = random.Next(0, Map.Width);
                var y = random.Next(0, Map.Height);
                if (Map[x, y].Value == 0)
                {
                    var value = random.NextDouble() < 0.9 ? 2 : 4;
                    Map[x, y] = new Tile(value);
                    break;
                }
            }
        }
    }
}