using System.Drawing;

namespace TwoThousandsFourtyEight
{
    public class Cell
    {
        public int Value;
        public Color Color;
        
        public Cell(int value)
        {
            Value = value;
            Color = GetColor();
        }

        private Color GetColor()
        {
            return Color.Bisque;
        }
    }
}