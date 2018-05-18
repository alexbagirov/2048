using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game2048
{
    public partial class Game
    {
        public Stack<List<Transition>> Transitions { get; } = new Stack<List<Transition>>();
        public Stack<Direction> Moves { get; } = new Stack<Direction>();
        private readonly Dictionary<Direction, MovementPresets> preset =
            new Dictionary<Direction, MovementPresets>();
        
        public bool MakeMove(Direction direction)
        {
            var transitions = new List<Transition>();
            var moveFlag = false;
            var vector = preset[direction].Vector;
            var xRange = preset[direction].XRange;
            var yRange = preset[direction].YRange;
            var mergedTiles = new HashSet<Tile>();
            
            foreach (var y in yRange)
            foreach (var x in xRange)
            {
                if (this[x, y].Value == 0)
                    continue;
                var curPos = new Point(x, y);
                var moved = SlideTile(curPos, vector, mergedTiles, transitions);
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

        private bool SlideTile(Point curPos, Point vector, HashSet<Tile> mergedTiles,
            List<Transition> transitions)
        {
            var merged = false;
            var startPos = curPos;
            var startValue = this[startPos].Value;
            var moved = false;
            
            while (true)
            {
                var nextPos = new Point(curPos.X + vector.X, curPos.Y + vector.Y);
                if (!InBounds(nextPos) || (this[nextPos].Value != 0 &&
                                           (this[nextPos].Value != this[curPos].Value ||
                                            mergedTiles.Contains(this[nextPos]))))
                    break;
                var curValue = this[curPos].Value;
                var nextValue = this[nextPos].Value;
                MoveValue(curPos, nextPos);
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
                    merged ? Condition.Merged : Condition.Moved));
            return moved;
        }

        private void MoveValue(Point from, Point to)
        {
            this[to].AddValue(this[from].Value);
            this[from].ChangeValue(0);
            if (emptyPositions.Contains(to))
                emptyPositions.Remove(to);
            emptyPositions.Add(from);
        }

        public void Undo()
        {
            if (Moves.Count == 0)
                return;
            var lastMove = Moves.Pop();
            var transitions = Transitions.Pop();
            var orderedTransitions = transitions
                .OrderByDescending(t => t.Condition==Condition.Appeared)
                .ThenBy(t =>
                {
                    switch (lastMove)
                    {
                        case Direction.Right:
                            return t.Start.X;
                        case Direction.Left:
                            return -t.Start.X;
                        case Direction.Down:
                            return t.Start.Y;
                        default:
                            return -t.Start.Y;
                    }
                });
            
            foreach (var transition in orderedTransitions)
            {
                if (transition.Condition == Condition.Appeared)
                {
                    ChangeTile(transition.Finish, 0);
                    continue;
                }
                if (transition.StartValue != this[transition.Finish].Value)
                {
                    Score -= this[transition.Finish].Value;
                    var previousValue = this[transition.Finish].Value / 2;
                    ChangeTile(transition.Finish, previousValue);
                    ChangeTile(transition.Start, previousValue);
                }
                else
                    MoveValue(transition.Finish, transition.Start);
            }
        }
        
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
    }
}