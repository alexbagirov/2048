﻿using System.Windows.Forms;

namespace Game2048
{
    internal class Program
    {
        public static void Main()
        {
            var form = new GameForm(4, 4);
            Application.Run(form);
        }
    }
}