using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorStateNameAttribute))]
    public class AnimatorStateNameDrawer : RuntimeAnimatorControllerPropertyDrawer<AnimatorStateNameAttribute>
    {
        public AnimatorStateNameDrawer() : base(SerializedPropertyType.String, SerializedPropertyType.Integer)
        {
        }

        protected override string GetPropertyPath(AnimatorStateNameAttribute attribute)
        {
            return attribute.AnimatorField;
        }

        protected override void DrawAnimatorControllerProperty(Rect position, SerializedProperty property, AnimatorController animatorController)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    var propertyIntValue = property.hasMultipleDifferentValues
                        ? "-"
                        : property.intValue == 0
                            ? ""
                            : animatorController.layers
                                .SelectMany(x => x.stateMachine.states)
                                .FirstOrDefault(x => x.state.nameHash == property.intValue)
                                .state.name;

                    var contentInt = String.IsNullOrEmpty(propertyIntValue) ? new GUIContent("<None>") : new GUIContent(propertyIntValue);
                    if (GUI.Button(position, contentInt, EditorStyles.popup))
                    {
                        StateSelectorInt(property, animatorController);
                    }

                    break;
                case SerializedPropertyType.String:
                    var propertyStringValue = property.hasMultipleDifferentValues ? "-" : property.stringValue;
                    var content = String.IsNullOrEmpty(propertyStringValue) ? new GUIContent("<None>") : new GUIContent(propertyStringValue);
                    if (GUI.Button(position, content, EditorStyles.popup))
                    {
                        StateSelectorString(property, animatorController);
                    }

                    break;
                default:
                    break;
            }
        }

        private static void StateSelectorString(SerializedProperty property, AnimatorController animatorController)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("<None>"),
                string.IsNullOrEmpty(property.stringValue),
                static x =>
                {
                    var prop = (SerializedProperty)x;
                    prop.stringValue = "";
                    prop.serializedObject.ApplyModifiedProperties();
                },
                property);

            foreach (var layer in animatorController.layers)
            {
                var stateNamePrefix = layer.name + "/";
                foreach (var childState in layer.stateMachine.states)
                {
                    var stateName = childState.state.name;
                    menu.AddItem(new GUIContent(stateNamePrefix + stateName),
                        stateName == property.stringValue,
                        StringPropertyPair.HandlePairObjectSelect,
                        new StringPropertyPair(stateName, property));
                }
            }

            menu.ShowAsContext();
        }

        private static void StateSelectorInt(SerializedProperty property, AnimatorController animatorController)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("<None>"),
                property.intValue == 0,
                static x =>
                {
                    var prop = (SerializedProperty)x;
                    prop.intValue = 0;
                    prop.serializedObject.ApplyModifiedProperties();
                },
                property);

            foreach (var layer in animatorController.layers)
            {
                var stateNamePrefix = layer.name + "/";
                foreach (var childState in layer.stateMachine.states)
                {
                    var stateName = childState.state.name;
                    menu.AddItem(new GUIContent(stateNamePrefix + stateName),
                        Animator.StringToHash(stateName) == property.intValue,
                        StringPropertyPair.HandlePairObjectSelectInt,
                        new StringPropertyPair(stateName, property));
                }
            }

            menu.ShowAsContext();
        }
    }
}