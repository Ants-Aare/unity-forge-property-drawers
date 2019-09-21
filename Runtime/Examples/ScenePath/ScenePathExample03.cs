﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace UnityForge.PropertyDrawers
{
    public class ScenePathExample03 : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField, ScenePath(fromBuildSettings: false)]
        private string exampleScenePath;
#pragma warning restore 0649

#if UNITY_EDITOR
        [ContextMenu("Open Scene At Path")]
        private void OpenSceneAtPath()
        {
            // Executing this method in Unity 2017.1.1f1 results in console message
            // "Assertion failed: Assertion failed on expression: 'm_InstanceID != InstanceID_None'"
            EditorSceneManager.OpenScene(exampleScenePath, OpenSceneMode.Single);
        }

        [ContextMenu("Open Scene At Path Additive")]
        private void OpenSceneAtPathAdditive()
        {
            EditorSceneManager.OpenScene(exampleScenePath, OpenSceneMode.Additive);
        }

        [ContextMenu("Ping Scene At Path")]
        private void PingSceneAtPath()
        {
            var exampleSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(exampleScenePath);
            EditorGUIUtility.PingObject(exampleSceneAsset);
        }
#endif
    }
}
