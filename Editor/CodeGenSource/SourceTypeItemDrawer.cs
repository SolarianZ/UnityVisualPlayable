using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.CodeGenSource.Editor
{
    public class SourceTypeItemDrawer : VisualElement
    {
        /// <summary>
        /// Index of this visual element in the ListView.
        /// </summary>
        public int ItemIndex { get; private set; } = -1;

        /// <summary>
        /// Index of the target type in the candidate type name list.
        /// </summary>
        public int TypeIndex { get; private set; } = -1;

        /// <summary>
        /// Name of the target type.
        /// </summary>
        public string TypeName
        {
            get
            {
                if (TypeIndex < 0) return null;
                return _candidateTypeNames[TypeIndex];
            }
        }

        public event Action<SourceTypeItemInfo> OnTypeNameChanged;


        /// <summary>
        /// Click this button to select target type.
        /// </summary>
        private readonly Button _typeNameButton;

        private IReadOnlyList<string> _candidateTypeNames;

        private IReadOnlyList<string> _disabledTypeNames;


        public SourceTypeItemDrawer()
        {
            style.flexGrow = 1;

            _typeNameButton = new Button
            {
                style =
                {
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleLeft,
                }
            };
            _typeNameButton.clickable.clickedWithEventInfo += ShowTypeNamePopupList;

            Add(_typeNameButton);
        }

        public void Initialize(int itemIndex, IReadOnlyList<string> candidateTypeNames,
            IReadOnlyList<string> disabledTypeNames, int selectedTypeIndex)
        {
            ItemIndex = itemIndex;
            TypeIndex = selectedTypeIndex;
            _candidateTypeNames = candidateTypeNames;
            _disabledTypeNames = disabledTypeNames;
            _typeNameButton.text = TypeName;
        }

        private void ShowTypeNamePopupList(EventBase evt)
        {
            if (_candidateTypeNames == null || _candidateTypeNames.Count == 0)
            {
                Debug.LogWarning("Candidate type names is empty.");
                return;
            }

            var menu = new GenericDropdownMenu();
            for (var i = 0; i < _candidateTypeNames.Count; i++)
            {
                var typeName = _candidateTypeNames[i];
                var isTypeNameSelected = i == TypeIndex;
                if (_disabledTypeNames?.Contains(typeName) ?? false)
                {
                    menu.AddDisabledItem(typeName, isTypeNameSelected);
                }
                else
                {
                    menu.AddItem(typeName, isTypeNameSelected, OnSelectTypeName, i);
                }
            }

            menu.DropDown(new Rect(evt.originalMousePosition, Vector2.zero), _typeNameButton);
        }

        private void OnSelectTypeName(object typeIndexObject)
        {
            var typeIndex = (int)typeIndexObject;
            if (typeIndex != TypeIndex)
            {
                TypeIndex = typeIndex;
                _typeNameButton.text = TypeName;

                OnTypeNameChanged?.Invoke(new SourceTypeItemInfo(ItemIndex, TypeIndex));
            }
        }


        public readonly struct SourceTypeItemInfo
        {
            /// <summary>
            /// Index of this visual element in the ListView.
            /// </summary>
            public readonly int ItemIndex;

            /// <summary>
            /// Index of the target type in the candidate type name list.
            /// </summary>
            public readonly int TypeIndex;


            public SourceTypeItemInfo(int itemIndex, int typeIndex)
            {
                ItemIndex = itemIndex;
                TypeIndex = typeIndex;
            }
        }
    }
}