using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game2048
{
    public class MovementPresets
    {
        public MovementPresets(Point vector, IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            Vector = vector;
            XRange = xRange;
            YRange = yRange;
        }
        public GameMap Map;
        public Point Vector;
        public IEnumerable<int> XRange;
        public IEnumerable<int> YRange;
    }
}
