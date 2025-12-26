using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference
{
    [SerializeField] private Object sceneAsset;
    [SerializeField] private string sceneName = "";

    public string SceneName
    {
        get
        {
        #if UNITY_EDITOR
            return sceneAsset != null ? sceneAsset.name : sceneName;
        #else
            return sceneName;
        #endif
        }
    }

    #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(SceneReference))]
        public class SceneReferenceDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var sceneAsset = property.FindPropertyRelative("sceneAsset");
                var sceneName = property.FindPropertyRelative("sceneName");

                EditorGUI.BeginChangeCheck();
                var newScene = EditorGUI.ObjectField(position, label, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

                if (EditorGUI.EndChangeCheck())
                {
                    sceneAsset.objectReferenceValue = newScene;
                    if (newScene != null)
                    {
                        sceneName.stringValue = newScene.name;
                    }
                }
            }
        }
    #endif
}