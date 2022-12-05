using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class InputTriggerLabelAttribute : System.Attribute
    {
        public string Label { get; }

        public InputTriggerLabelAttribute(string label)
        {
            Label = label;
        }
    }
}