using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ExcludeFromNodeAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class ExcludeMethodFromNodeAttribute : System.Attribute
    {
        public string MethodName { get; }

        public ExcludeMethodFromNodeAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}