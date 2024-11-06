using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapUpdate))]
public class MapUpdateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        MapUpdate mapUpdate = target as MapUpdate;
        
        if (GUILayout.Button("SetStartMap"))
        {
            Debug.Log("マップセット");
            mapUpdate.SetStartMap(mapUpdate.StartMapName);
        }

        if (GUILayout.Button("ResetStartMap"))
        {
            mapUpdate.ResetStartMap();
        }
    }
}
