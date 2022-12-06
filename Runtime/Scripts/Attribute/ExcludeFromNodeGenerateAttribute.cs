using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct)]
    internal class ExcludeFromNodeGenerateAttribute : System.Attribute
    {
    }
}