using UnityEngine;
using UnityEditor;
using Hexiled.World.Components;
using Hexiled.World.SO;
using Hexiled.World.Events;


[CustomEditor(typeof(GridDrawer))]
public class GridVisibilityEditor : Editor
{
    [SerializeField]
    public BoolSO showGrid;
    [SerializeField]
    public IntSO editorMode;
    [SerializeField]
    public GUIArrayVariable terrainVisibility;
    bool autoUpdate;
    [SerializeField]
    public VoidEventSO TerrainSettingsChanged;
    [SerializeField]
    public BoolEventSO AutoUpdateChanged;
    [SerializeField]
    public BoolSO terrainVisible;

    private void OnEnable()
    {
        //if (showGrid == null)
        //    showGrid = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "ShowGrid" + ".asset");
        //if (terrainVisibility == null)
        //    terrainVisibility = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons + "TerrainVisibility" + ".asset");
        //if (editorMode == null)
        //    editorMode = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "EditorMode" + ".asset");
        //if (AutoUpdateChanged == null)
        //    AutoUpdateChanged = (BoolEvent)EditorGUIUtility.LoadRequired(InternalPaths.boolevents + "AutoUpdateChanged" + ".asset");
        //if (TerrainSettingsChanged == null)
        //    TerrainSettingsChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "TerrainUpdate" + ".asset");
        //if (terrainVisible == null)
        //    terrainVisible = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "TerrainVisible" + ".asset");
        //SceneView.duringSceneGui += OnScene;
    }
    //void OnDestroy() => SceneView.duringSceneGui -= OnScene;
    //void OnDisable() => SceneView.duringSceneGui -= OnScene;

    private void OnSceneGUI()
    {
        int h = editorMode.Value == 0 ? 40 : 0;
        int w = terrainVisible.Value ? 80 : 40;
        //EditorGUI.BeginChangeCheck();
        GUILayout.BeginArea(new Rect(Screen.width * .75f + w, h, 36, 36));
            showGrid.Value = GUILayout.Toggle(showGrid.Value, terrainVisibility.GuiContentArray()[1], "Button");
            GUILayout.EndArea();
        //if (EditorGUI.EndChangeCheck())
        //{
        //    showGrid.InitialValue = showGrid.Value;
        //    EditorUtility.SetDirty(showGrid);
        //}

        if (terrainVisible.Value)
        {
        GUILayout.BeginArea(new Rect(Screen.width * .75f + 40, h, 36, 36));

        EditorGUI.BeginChangeCheck();
        autoUpdate = GUILayout.Toggle(autoUpdate, terrainVisibility.GuiContentArray()[2], "Button");
        if (EditorGUI.EndChangeCheck())
        {
            AutoUpdateChanged.Event.Invoke(autoUpdate);
            TerrainSettingsChanged.Event.Invoke();
        }
        GUILayout.EndArea();
        }
    }
}
