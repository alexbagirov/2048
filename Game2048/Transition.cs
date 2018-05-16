using System.Collections.Generic;
using System.Drawing;

namespace Game2048
{
    public class Transition
    {
        public readonly Point Start;
        public readonly Point Finish;
        public readonly int StartValue;
        public readonly Condition Condition;
        public Transition(Point start, Point finish, int startValue, Condition condition)
        {
            Start = start;
            Finish = finish;
            StartValue = startValue;
            Condition = condition;
        }

        public override bool Equals(object obj)
        {
            var transition = obj as Transition;
            return Start == transition.Start && Finish == transition.Finish
                && StartValue == transition.StartValue;
        }
    }

    public enum Condition
    {
        Merged,
        Moved,
        Appeared
    }
}
