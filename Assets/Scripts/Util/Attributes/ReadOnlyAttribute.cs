using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
/// <summary>
/// Draws ReadOnlyAttribute
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    /// <summary>
    /// Includes children in GetPropertyHeight calucation
    /// </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    /// <summary>
    /// Makes a "read only" serialized field for this GUI
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Disabling the gui makes it uneditable in inspector, renable after to see it
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif