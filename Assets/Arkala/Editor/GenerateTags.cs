using System;
using ClickNext.Scripts;
using UnityEditor;
using UnityEngine;
namespace ClickNext.Editor
{
    [InitializeOnLoad]
    public class GenerateTags
    {
        static GenerateTags()
        {
            Debug.Log("Generated new tags and layers.");
            var tagManager = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/TagManager.asset") [0]);

            var tagsProp = tagManager.FindProperty ("tags");
            var tags = (TagType[])Enum.GetValues(typeof(TagType));
            tagsProp.arraySize = tags.Length;
            foreach (var tag in tags)
            {
                if (tag == TagType.Untagged)
                    continue;

                var sp = tagsProp.GetArrayElementAtIndex((int) tag);
                sp.stringValue = tag.ToString();
            }

            SerializedProperty layersProp = tagManager.FindProperty("layers");
            var layer = layersProp.GetArrayElementAtIndex((int)LayerType.Blueprint);
            layer.stringValue = LayerType.Blueprint.ToString();

            tagManager.ApplyModifiedProperties();
        }
    }
}
