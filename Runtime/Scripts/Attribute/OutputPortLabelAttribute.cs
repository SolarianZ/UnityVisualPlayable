using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class OutputPortLabelAttribute : System.Attribute
    {
        public string Label { get; }

        public OutputPortLabelAttribute(string label)
        {
            Label = label;
        }
    }
}