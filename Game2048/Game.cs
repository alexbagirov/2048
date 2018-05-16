using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game2048
{
    public class Game
    {
        private readonly Tile[,] map;
        public readonly int Width;
        public readonly int Height;
        private HashSet<Point> emptyPositions = new HashSet<Point>();
        public int Score { get; private set; }
        public Stack<List<Transition>> Transitions = new Stack<List<Transition>>();
        private Stack<Direction> Moves = new Stack<Direction>();

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

        private readonly Dictionary<Direction, MovementPresets> preset =
            new Dictionary<Direction, MovementPresets>();

        public Tile this[int x, int y] => map[x, y];

        public Tile this[Point point] => map[point.X, point.Y];

        public void AddTile(Point point, int value)
        {
            this[point].ChangeValue(value);
            if (value != 0 && emptyPositions.Contains(point))
                emptyPositions.Remove(point);
            if (value == 0)
                emptyPositions.Add(point);
        }

        public void MoveTile(Point from, Point to)
        {
            this[to].AddValue(this[from].Value);
            this[from].ChangeValue(0);
            if (emptyPositions.Contains(to))
                emptyPositions.Remove(to);
            emptyPositions.Add(from);
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
                    s.Append(this[i, j].Value.ToString() + ' ');
                }

                s.Append('\n');
            }

            return s.ToString();
        }

        public bool IsEmpty(Point point) => emptyPositions.Contains(point);
        
        private void FillMovePresets()
        {
            preset[Direction.Up] = new MovementPresets(new Point(0, -1), 
                Enumerable.Range(0, Width).ToArray(), Enumerable.Range(0, Height).ToArray());
            preset[Direction.Left] = new MovementPresets(new Point(-1, 0), 
                Enumerable.Range(0, Width).ToArray(), Enumerable.Range(0, Height).ToArray());
            preset[Direction.Down] = new MovementPresets(new Point(0, 1), 
                Enumerable.Range(0, Width).ToArray(), Enumerable.Range(0, Height).Reverse().ToArray());
            preset[Direction.Right] = new MovementPresets(new Point(1, 0), 
                Enumerable.Range(0, Width).Reverse().ToArray(), Enumerable.Range(0, Height).ToArray());
        }

        private bool TrySimpleMove(Point curPos, Point vector, 
            HashSet<Tile> mergedTiles, List<Transition> transitions)
        {
            var merged = false;
            var startPos = curPos;
            var startValue = this[startPos].Value;
            var moved = false;
            while (true)
            {
                var nextPos = new Point(curPos.X + vector.X, curPos.Y + vector.Y);
                if (!InBounds(nextPos) || (this[nextPos].Value != 0 && 
                    (this[nextPos].Value != this[curPos].Value || mergedTiles.Contains(this[nextPos]))))
                    break;
                var curValue = this[curPos].Value;
                var nextValue = this[nextPos].Value;
                MoveTile(curPos, nextPos);
                curPos = nextPos;
                moved = true;
                if (nextValue == curValue)
                {
                    mergedTiles.Add(this[curPos]);
                    Score += this[curPos].Value;
                    merged = true;
                    break;
                }
            }
            if (moved)
                transitions.Add(new Transition(startPos, curPos, startValue, 
                    merged? Condition.Merged : Condition.Moved));
            return moved;
        }

        public bool TryMove(Direction direction)
        {
            var transitions = new List<Transition>();
            var moveFlag = false;
            var preset = this.preset[direction];
            var mergedTiles = new HashSet<Tile>();
            foreach (var y in preset.YRange)
                foreach (var x in preset.XRange)
                {
                    if (this[x, y].Value == 0) 
                        continue;
                    var curPos = new Point(x, y);
                    var moved = TrySimpleMove(curPos, preset.Vector, mergedTiles, transitions);
                    if (moved)
                        moveFlag = true;
                }
            if (moveFlag)
            {
                Transitions.Push(transitions);
                Moves.Push(direction);
            }
            return moveFlag;
        }

        public void AddRandomTile()
        {
            var random = new Random();
            var point = emptyPositions.ElementAt(random.Next(emptyPositions.Count));
            var value = random.NextDouble() < 0.9 ? 2 : 4;
            AddTile(point, value);
            Transitions.Peek().Add(new Transition(new Point(-1, -1), point, value, Condition.Appeared));
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

        public void Undo()
        {
            if (Moves.Count == 0)
                return;
            var lastMove = Moves.Pop();
            var transitions = Transitions.Pop();
            foreach (var transition in transitions.Where(t => t.Condition == Condition.Appeared))
                AddTile(transition.Finish, 0);
            var orderedTransitions = transitions
                .Where(t => t.Condition != Condition.Appeared)
                .OrderBy(t =>
                {
                    if (lastMove == Direction.Right)
                        return t.Start.X;
                    return -t.Start.X;
                })
                .ThenBy(t =>
                {
                    if (lastMove == Direction.Down)
                        return t.Start.Y;
                    return -t.Start.Y;
                });
            foreach (var transition in orderedTransitions)
            {
                if (transition.StartValue != this[transition.Finish].Value)
                {
                    Score -= this[transition.Finish].Value;
                    var previousValue = this[transition.Finish].Value / 2;
                    AddTile(transition.Finish, previousValue);
                    AddTile(transition.Start, previousValue);
                }
                else
                    MoveTile(transition.Finish, transition.Start);  
            }
        }
    }
}
