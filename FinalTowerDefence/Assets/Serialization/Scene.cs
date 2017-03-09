using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code adapted from http://answers.unity3d.com/answers/1204071/view.html
//Purpose: allow the unity editor to serialize scenes

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Scene
{
    [SerializeField]
    private Object m_SceneAsset;
    [SerializeField]
    private string m_SceneName = "";
    public string SceneName
    {
        get {
            return m_SceneName;
        }
    }

    // makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(Scene sceneField)
    {
        return sceneField.SceneName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Scene))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        SerializedProperty sceneAsset = _property.FindPropertyRelative("m_SceneAsset");
        SerializedProperty sceneName = _property.FindPropertyRelative("m_SceneName");
        _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (sceneAsset != null)
        {
            sceneAsset.objectReferenceValue = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
            if (sceneAsset.objectReferenceValue != null)
            {
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
            }
        }
        EditorGUI.EndProperty();
    }
}
#endif
