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
        
        public Game(int width, int height)
        {
            Map = new GameMap(width, height);
            FillMovePresets();
            NewTile();
            NewTile();
        }

        public Dictionary<Direction, MovementPresets> Presets = new Dictionary<Direction, MovementPresets>();

        public void FillMovePresets()
        {
            Presets[Direction.Up] = new MovementPresets(new Point(0, -1), Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height));
            Presets[Direction.Left] = new MovementPresets(new Point(-1, 0), Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height));
            Presets[Direction.Down] = new MovementPresets(new Point(0, 1), Enumerable.Range(0, Map.Width), Enumerable.Range(0, Map.Height).Reverse());
            Presets[Direction.Right] = new MovementPresets(new Point(1, 0), Enumerable.Range(0, Map.Width).Reverse(), Enumerable.Range(0, Map.Height));
        }

        public bool TryMove(Direction direction)
        {
            var moved = false;
            
            var presets = Presets[direction];
            var xRange = presets.XRange;
            var yRange = presets.YRange;
            
            var mergedTiles = new HashSet<Tile>();
            
            foreach (var y in yRange)
            {
                foreach (var x in xRange)
                {
                    if (Map[x, y].Value == 0) 
                        continue;
                    
                    var curPos = new Point(x, y);
                    while (true)
                    {
                        var nextPos = new Point(curPos.X + presets.Vector.X, curPos.Y + presets.Vector.Y);
                        if (!Map.InBounds(nextPos))
                            break;
                        
                        if (Map[nextPos].Value != 0)
                        {
                            if (Map[nextPos].Value == Map[curPos].Value && !mergedTiles.Contains(Map[nextPos]))
                            {
                                Map.MoveTile(curPos, nextPos);
                                curPos = nextPos;
                                mergedTiles.Add(Map[curPos]);
                                moved = true;
                            }
                            break;
                        }
                        
                        Map.MoveTile(curPos, nextPos);
                        curPos = nextPos;
                        moved = true;
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

        public void NewTile()
        {
            var random = new Random();
            Map.AddTile(Map.GetEmptyTilePosition(), random.NextDouble() < 0.9 ? 2 : 4);
        }
    }
}