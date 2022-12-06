using GBG.VisualPlayable.Attribute;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Playables;

namespace GBG.VisualPlayable.Editor.Utils
{
    public static class TypeTool
    {
        public static Type[] CollectPlayableTypes()
        {
            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsPublic && !type.IsGenericType && !type.IsInterface &&
                      (type == typeof(PlayableGraph) || typeof(IPlayable).IsAssignableFrom(type))
                select type;

            return types.ToArray();
        }

        public static Type[] CollectVisualPlayableTypes()
        {
            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsPublic && !type.IsGenericType && !type.IsInterface &&
                      type.BaseType != typeof(Unit) &&
                      type.Assembly == typeof(VisualPlayableExtensions).Assembly &&
                      type.GetAttribute<ExcludeFromNodeGenerateAttribute>() == null
                select type;

            return types.ToArray();
        }
    }
}