using System.Collections.Generic;
using System.Drawing;

namespace TwoThousandsFourtyEight
{
    public class Colors
    {
        private static readonly Dictionary<int, Color> NumbersColors = new Dictionary<int, Color>
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

        public static Color GetColor(int value) => NumbersColors[value];
    }
}