using System.Collections.Generic;
using System.Drawing;

namespace Game2048
{
    public class Tile
    {
        public int Value { get; private set; }
        public Color Color { get; private set; }
        public bool Hidden { get; private set; }
        
        public Tile(int value = 0)
        {
            Value = value;
            Color = GetColor(Value);
            if (Value == 0)
                Hidden = true;
        }

        public void ChangeValue(int value)
        {
            Value = value;
            Color = GetColor(Value);
            Hidden = Value == 0;
        }

        public void AddValue(int value) => ChangeValue(Value + value);
        
        private static readonly Dictionary<int, Color> Colors = new Dictionary<int, Color>
        {
            {0, ColorTranslator.FromHtml("#bbada0")},
            {2, ColorTranslator.FromHtml("#eee4da")},
            {4, ColorTranslator.FromHtml("#ede0c8")},
            {8, ColorTranslator.FromHtml("#f2b179")},
            {16, ColorTranslator.FromHtml("#f59563")},
            {32, ColorTranslator.FromHtml("#f67c5f")},
            {64, ColorTranslator.FromHtml("#f65e3b")},
            {128, ColorTranslator.FromHtml("#edcf72")},
            {256, ColorTranslator.FromHtml("#edcc61")},
            {512, ColorTranslator.FromHtml("#edc850")},
            {1024, ColorTranslator.FromHtml("#edc53f")},
            {2048, ColorTranslator.FromHtml("#edc22e")}
        };

        public static Color GetColor(int value) => Colors[value];
    }
}