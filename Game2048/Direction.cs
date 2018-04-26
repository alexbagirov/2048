namespace Game2048
{
    public enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }

    public class DirectionParser
    {
        public static Direction? Parse(string s)
        {
            switch (s)
            {
                case "W":
                    return Direction.Up;
                case "A":
                    return Direction.Left;
                case "S":
                    return Direction.Down;
                case "D":
                    return Direction.Right;
                default:
                    return null;
            }
        }
    }
}
