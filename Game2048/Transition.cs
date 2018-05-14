using System.Collections.Generic;
using System.Drawing;

namespace Game2048
{
    public class Transition
    {
        public readonly Point Start;
        public readonly Point Finish;
        public readonly int StartValue;
        public Transition(Point start, Point finish, int startValue)
        {
            Start = start;
            Finish = finish;
            StartValue = startValue;
        }

        public override bool Equals(object obj)
        {
            var transition = obj as Transition;
            return Start == transition.Start && Finish == transition.Finish
                && StartValue == transition.StartValue;
        }

    }
}
