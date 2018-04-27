using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace Game2048
{
    public class Game
    {
        private readonly GameMap map;
        public bool Moved;
        
        public Game(int width, int height)
        {
            map = new GameMap(width, height);
            FillMovePresets();
            AddNewTile();
            AddNewTile();
        }

        private readonly Dictionary<Direction, MovementPresets> presets = 
            new Dictionary<Direction, MovementPresets>();

        private void FillMovePresets()
        {
            presets[Direction.Up] = new MovementPresets(new Point(0, -1), 
                Enumerable.Range(0, map.Width), Enumerable.Range(0, map.Height));
            presets[Direction.Left] = new MovementPresets(new Point(-1, 0), 
                Enumerable.Range(0, map.Width), Enumerable.Range(0, map.Height));
            presets[Direction.Down] = new MovementPresets(new Point(0, 1), 
                Enumerable.Range(0, map.Width), Enumerable.Range(0, map.Height).Reverse());
            presets[Direction.Right] = new MovementPresets(new Point(1, 0), 
                Enumerable.Range(0, map.Width).Reverse(), Enumerable.Range(0, map.Height));
        }

        public bool TryMove(Direction direction)
        {
            var moved = false;
            
            var presets = this.presets[direction];
            var xRange = presets.XRange;
            var yRange = presets.YRange;
            
            var mergedTiles = new HashSet<Tile>();
            
            foreach (var y in yRange)
            {
                foreach (var x in xRange)
                {
                    if (map[x, y].Value == 0) 
                        continue;
                    
                    var curPos = new Point(x, y);
                    while (true)
                    {
                        var nextPos = new Point(curPos.X + presets.Vector.X, curPos.Y + presets.Vector.Y);
                        if (!map.InBounds(nextPos))
                            break;
                        
                        if (map[nextPos].Value != 0)
                        {
                            if (map[nextPos].Value == map[curPos].Value && !mergedTiles.Contains(map[nextPos]))
                            {
                                map.MoveTile(curPos, nextPos);
                                curPos = nextPos;
                                mergedTiles.Add(map[curPos]);
                                moved = true;
                            }
                            break;
                        }
                        
                        map.MoveTile(curPos, nextPos);
                        curPos = nextPos;
                        moved = true;
                    }
                }
            }
            return moved;
        }

        public void Print()
        {
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                    Console.Write(map[x, y].Value.ToString().PadRight(4));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void AddNewTile()
        {
            var random = new Random();
            map.AddTile(map.GetEmptyTilePosition(), random.NextDouble() < 0.9 ? 2 : 4);
        }

        public bool HasEnded()
        {
            for (var i = 0; i < map.Width; i++)
            {
                for (var j = 0; j < map.Height; j++)
                {
                    if (TileHasEqualNeighbours(i, j) || map[i, j].Value == 0)
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
                    if (!map.InBounds(neighbour) || neighbour == point || dx != 0 && dy != 0)
                        continue;

                    if (map[neighbour].Value == map[point].Value)
                        return true;
                }
            }

            return false;
        }
    }
}