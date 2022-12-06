using GBG.VisualPlayable.Editor.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GBG.VisualPlayable.Editor.Setup
{
    public static class SetupVisualPlayable
    {
        private static readonly IReadOnlyList<AssemblyName> _nodeLibrary = new[]
        {
            typeof(VisualPlayableExtensions).Assembly.GetName(),
        };


        [MenuItem("Tools/Bamboo/Visual Playable/Setup Visual Playable")]
        public static void Setup()
        {
            SetupNodeLibrary();
            SetupTypeOptions();
            UnitBase.Rebuild();
        }

        public static void SetupNodeLibrary()
        {
            var boltNodeLibraryChanged = false;
            foreach (var asmName in _nodeLibrary)
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
            var allTypes = TypeTool.CollectPlayableTypes().Concat(TypeTool.CollectVisualPlayableTypes());
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
    }
}