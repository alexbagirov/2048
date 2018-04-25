using System.Drawing;

namespace TwoThousandsFourtyEight
{
    public class Cell
    {
        public int Value;
        private Color color;
        private bool hidden;
        
        public Cell(int value)
        {
            Value = value;
            color = Tile.GetColor(Value);

            if (Value == 0)
                hidden = true;
        }

        public Color GetColor() => color;
        public bool IsHidden() => hidden;

        public void ChangeColor(int value)
        {
            if (Tile.Exist(value))
            {
                color = Tile.GetColor(value);
                hidden = Value == 0;  
            }
        }
    }
}