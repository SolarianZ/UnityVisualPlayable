using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Editor.CodeGenSource.Editor
{
    // [CreateAssetMenu(menuName = "Bamboo/Code Gen Config Asset", fileName = "New CodeGenConfig")]
    public class CodeGenConfigAsset : ScriptableObject
    {
        public const string SOURCE_NAMESPACE = "UnityEngine.Playables";

        public List<string> SourceTypes => _sourceTypes;

        [SerializeField]
        private List<string> _sourceTypes = new List<string>();

        public static bool FilterType(Type type)
        {
            return !type.IsAbstract &&
                   !type.IsInterface &&
                   !type.IsEnum &&
                   type.IsPublic &&
                   type.Namespace!.Equals(typeof(Playable).Namespace);
        }
    }
}