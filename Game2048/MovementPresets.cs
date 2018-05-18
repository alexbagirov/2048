using System.Drawing;

namespace Game2048
{
    public class MovementPresets
    {
        public Point Vector;
        public int[] XRange;
        public int[] YRange;
        
        public MovementPresets(Point vector, int[] xRange, int[] yRange)
        {
            Vector = vector;
            XRange = xRange;
            YRange = yRange;
        }
    }
}
