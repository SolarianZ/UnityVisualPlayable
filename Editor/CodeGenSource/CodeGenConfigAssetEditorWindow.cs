using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace GBG.VisualPlayable.Editor.CodeGen
{
    public class CodeGenConfigAssetEditorWindow : EditorWindow
    {
        [OnOpenAsset()]
        static bool OnOpenAsset(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset is CodeGenConfigAsset codeGenConfigAsset)
            {
                var windows = Resources.FindObjectsOfTypeAll<CodeGenConfigAssetEditorWindow>();
                var window = windows.FirstOrDefault(wnd => wnd._target == codeGenConfigAsset);
                if (!window)
                {
                    window = GetWindow<CodeGenConfigAssetEditorWindow>();
                }

                window.OpenAsset(codeGenConfigAsset);
                window.Show();
                window.Focus();

                return true;
            }

            return false;
        }


        private CodeGenConfigAsset _target;

        private List<string> _candidateTypeNames;

        public void OpenAsset(CodeGenConfigAsset target)
        {
            if (!target)
            {
                Debug.LogError("Target asset is null, window closed.");
                Close();
                return;
            }

            _target = target;

            var assembly = typeof(Playable).Assembly;
            var types = (from type in assembly.GetTypes()
                where CodeGenConfigAsset.FilterType(type)
                select type).ToArray();
            _candidateTypeNames = (from type in types select type.FullName).ToList();

            _sourceTypeListView.itemsSource = _target.SourceTypes;

            titleContent = new GUIContent(_target.name);
        }


        #region Mono Messages

        private void OnEnable()
        {
            // Source type list
            _sourceTypeListView = new ListView
            {
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                selectionType = SelectionType.Single,
                headerTitle = "Source Type Names",
                showBorder = true,
                showFoldoutHeader = true,
                showBoundCollectionSize = false,
                showAddRemoveFooter = true,
                makeItem = MakeSourceTypeListViewItem,
                bindItem = BindSourceTypeListViewItem,
                unbindItem = UnbindSourceTypeListViewItem,
            };
            rootVisualElement.Add(_sourceTypeListView);
        }

        #endregion


        private ListView _sourceTypeListView;


        private VisualElement MakeSourceTypeListViewItem()
        {
            return new SourceTypeItemDrawer();
        }

        private void BindSourceTypeListViewItem(VisualElement sourceTypeItemDrawerElem, int itemIndex)
        {
            // unity-list-view__reorderable-item__container
            sourceTypeItemDrawerElem.parent.style.justifyContent = Justify.Center;

            var typeName = _target.SourceTypes[itemIndex];
            var typeIndex = _candidateTypeNames.IndexOf(typeName);
            var sourceTypeItemDrawer = (SourceTypeItemDrawer)sourceTypeItemDrawerElem;
            sourceTypeItemDrawer.Initialize(itemIndex, _candidateTypeNames,
                _target.SourceTypes, typeIndex);
            sourceTypeItemDrawer.OnTypeNameChanged += OnSelectedTypeNameChanged;
        }

        private void UnbindSourceTypeListViewItem(VisualElement sourceTypeItemDrawerElem, int itemIndex)
        {
            var sourceTypeItemDrawer = (SourceTypeItemDrawer)sourceTypeItemDrawerElem;
            sourceTypeItemDrawer.OnTypeNameChanged -= OnSelectedTypeNameChanged;
        }

        private void OnSelectedTypeNameChanged(SourceTypeItemDrawer.SourceTypeItemInfo sourceTypeItemInfo)
        {
            _target.SourceTypes[sourceTypeItemInfo.ItemIndex] = _candidateTypeNames[sourceTypeItemInfo.TypeIndex];
        }
    }
}