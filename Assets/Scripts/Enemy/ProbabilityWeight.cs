using System;
using UnityEngine;
using UnityEditor;

/// <summary> 攻撃の確立の配列に行動名も表示 </summary>
[Serializable]
public class Weight
{
    [SerializeField] private string _name;
    [SerializeField, Range(0, 100)] private int _probability;

    public Weight(string name) { _name = name; }
    public int Probability => _probability;
}
#if UNITY_EDITOR
public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ProbabilityWeight : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}
#endif