using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class NodeSuffixAttribute : System.Attribute
    {
        public string Suffix { get; }

        public NodeSuffixAttribute(string suffix)
        {
            Suffix = suffix;
        }
    }
}