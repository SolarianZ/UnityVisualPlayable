using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace GBG.VisualPlayable.Editor.Setup
{
    public static class SetupVisualPlayable
    {
        private static readonly IReadOnlyList<AssemblyName> _visualPlayableNodeLibrary = new[]
        {
            typeof(VisualPlayableExtensions).Assembly.GetName(),
        };

        private static readonly IReadOnlyList<Type> _visualPlayableTypeOptions = new[]
        {
            typeof(VisualPlayableExtensions),
            typeof(AnimationBrain),
            typeof(AnimationLayer),
            typeof(AnimationClipInfo),
        };


        [MenuItem("Window/Bamboo/Visual Playable/Setup Visual Playable")]
        public static void Setup()
        {
            SetupNodeLibrary();
            SetupTypeOptions();
            UnitBase.Rebuild();
        }

        public static void SetupNodeLibrary()
        {
            var boltNodeLibraryChanged = false;
            foreach (var asmName in _visualPlayableNodeLibrary)
            {
                var looseAsmName = (LooseAssemblyName)asmName;
                if (!BoltCore.Configuration.assemblyOptions.Contains(looseAsmName))
                {
                    BoltCore.Configuration.assemblyOptions.Add(looseAsmName);
                    Debug.Log($"Add {looseAsmName} to Visual Scripting node library.");
                    boltNodeLibraryChanged = true;
                }
            }

            if (boltNodeLibraryChanged)
            {
                var nodeLibraryMetadataName = nameof(BoltCore.Configuration.assemblyOptions);
                var nodeLibraryMetadata = BoltCore.Configuration.GetMetadata(nodeLibraryMetadataName);
                nodeLibraryMetadata.Save();
                Codebase.UpdateSettings();
            }
        }

        public static void SetupTypeOptions()
        {
            var boltTypeOptionsChanged = false;
            var allTypes = _visualPlayableTypeOptions.Concat(CollectPlayableTypes());
            foreach (var type in allTypes)
            {
                if (!BoltCore.Configuration.typeOptions.Contains(type))
                {
                    BoltCore.Configuration.typeOptions.Add(type);
                    Debug.Log($"Add {type.FullName} to Visual Scripting type options.");
                    boltTypeOptionsChanged = true;
                }
            }

            if (boltTypeOptionsChanged)
            {
                var typeOptionsMetadataName = nameof(BoltCore.Configuration.typeOptions);
                var typeOptionsMetadata = BoltCore.Configuration.GetMetadata(typeOptionsMetadataName);
                typeOptionsMetadata.Save();
                Codebase.UpdateSettings();
            }
        }

        private static Type[] CollectPlayableTypes()
        {
            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsPublic && !type.IsGenericType && !type.IsInterface &&
                      typeof(IPlayable).IsAssignableFrom(type)
                select type;

            return types.ToArray();
        }
    }
}