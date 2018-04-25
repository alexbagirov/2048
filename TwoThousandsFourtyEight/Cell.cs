using System.Drawing;

namespace TwoThousandsFourtyEight
{
    public class Cell
    {
        public int Value;
        private Color color;
        public bool Hidden { get; private set; }
        
        public Cell(int value)
        {
            Value = value;
            color = Colors.GetColor(Value);

            if (Value == 0)
                Hidden = true;
        }

        public Color GetColor() => color;

        public void ChangeColor(int value)
        {
            color = Colors.GetColor(value);
            Hidden = Value == 0;
        }
    }
}