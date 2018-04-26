using System.Collections.Generic;
using System.Drawing;

namespace Game2048
{
    public class MovementPresets
    {
        public Point Vector;
        public IEnumerable<int> XRange;
        public IEnumerable<int> YRange;
        
        public MovementPresets(Point vector, IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            Vector = vector;
            XRange = xRange;
            YRange = yRange;
        }
    }
}
