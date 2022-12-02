using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class InputTriggerNameAttribute : System.Attribute
    {
        public string Name { get; }

        public InputTriggerNameAttribute(string name)
        {
            Name = name;
        }
    }
}