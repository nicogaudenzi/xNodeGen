using UnityEngine;
using UnityEditor;
using Hexiled.World.Components;
using Hexiled.World.SO;
using Hexiled.World.Events;

[CustomEditor(typeof(ChunckDataButtons))]
public class ChunckDataButtonseditor : Editor
{
    [SerializeField]
    WorldDataContainer wdc;

    [SerializeField]
    Vector2IntSO currentchunk;
    [SerializeField]
    Vector2IntEventSO askForTerrainRepaint;


    void OnSceneGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width * 0f, Screen.height * .15f, 100, 40));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            wdc.WorldData.AddTerrainKey(currentchunk.Value);
            EditorUtility.SetDirty(wdc.WorldData);
            AssetDatabase.SaveAssets();
            
        }
        if (GUILayout.Button("-"))
        {
            wdc.WorldData.DeleteTerrainKey(currentchunk.Value);
            EditorUtility.SetDirty(wdc.WorldData);
            AssetDatabase.SaveAssets();
            askForTerrainRepaint.Event.Invoke(currentchunk.Value);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}