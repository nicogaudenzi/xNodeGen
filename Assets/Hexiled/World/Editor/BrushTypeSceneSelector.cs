using UnityEngine;
using UnityEditor;
using Hexiled.World.Components;
using Hexiled.World.SO;
using Hexiled.World.Events;
[CustomEditor(typeof(BrushTypeSelector))]
public class BrushTypeSceneSelector : Editor
{
    [SerializeField]
    WorldDataContainer wdc;
    [SerializeField]
    Vector2IntSO selectedChunk;
    [SerializeField]
    Vector3EventSO chunckChanged;
    [SerializeField]
    FillConditionsCheck fillConditions;
    [SerializeField]
    IntSO editorMode;
    [SerializeField]
    GUIArrayVariable terrainBrushTips, editorModeTips;
    [SerializeField]
    IntSO brushTypeSelected;
    [SerializeField]
    GUIArrayVariable brushTypeTips;
    [SerializeField]
    BoolSO useCircleVoxelBrush;
    [SerializeField]
    IntSO brushSize;
    [SerializeField]
    GUIArrayVariable voxelOptions;
    [SerializeField]
    GUIArrayVariable fillTips;
    [SerializeField]
    BoolSO terrainVisivility;
    [SerializeField]
    BoolEventSO terrainVisibilityChanged;
    [SerializeField]
    VoidEventSO brushSelectionChanged;
    private void OnEnable()
    {
        
        //if (wdc == null)
        //    wdc = (WorldDataContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+"World Data Container"+".asset");
        //if (selectedChunk == null)
        //    selectedChunk = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables+"SelectedChunk"+".asset");
        //if (chunckChanged == null)
        //    chunckChanged = (Vector3Event)EditorGUIUtility.LoadRequired(InternalPaths.vector3events+"ChunkChanged"+".asset");
        //if (fillConditions == null)
        //    fillConditions = (FillConditionsCheck)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+ "FillConditionsCheck" + ".asset");
        //if (editorMode == null)
        //    editorMode = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables+"EditorMode"+".asset");
        //if (terrainBrushTips == null)
        //    terrainBrushTips = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons+"TerrainTypeTips"+".asset");
        //if (editorModeTips == null)
        //    editorModeTips = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons+"editor Mode Tips"+".asset");
        //if (brushTypeSelected == null)
        //    brushTypeSelected = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables+"BrushTypeSelect"+".asset");
        //if (brushTypeTips == null)
        //    brushTypeTips = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons+"BrushTypeTips"+".asset");
        //if (useCircleVoxelBrush == null)
        //    useCircleVoxelBrush = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables+"UseCircleBrush"+".asset");
        //if (brushSize == null)
        //    brushSize = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables+"BrushSize"+".asset");
        //if (voxelOptions == null)
        //    voxelOptions = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons+"VoxelOptions"+".asset");
        //if (fillTips == null)
        //    fillTips = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons+"FillTips"+".asset");
        //if (terrainVisivility == null)
        //    terrainVisivility = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables+"TerrainVisible"+".asset");
        //if (terrainVisibilityChanged == null)
        //    terrainVisibilityChanged = (BoolEvent)EditorGUIUtility.LoadRequired(InternalPaths.boolevents+"TerrainVisibilityChanged"+".asset");
        //if (brushSelectionChanged == null)
        //    brushSelectionChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents+"brushSelectionChanged"+".asset");
        
        terrainVisibilityChanged.Event.AddListener(OnTerraiVisibilityChanged);
        chunckChanged.Event.AddListener(OnCunckChanged);
        if (Application.isPlaying) return;

    }
    void OnCunckChanged(Vector3 v) {
        Repaint();
    }
    void OnTerraiVisibilityChanged(bool b) {
       
        Repaint();
    }
     void OnSceneGUI()
    {
        Tools.current = Tool.View;
       
        Handles.BeginGUI();
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 20;
        GUI.color = Color.black;
        GUILayout.BeginArea(new Rect(Screen.width * 0f+5, Screen.height * 0f+5, 350, 50));
        GUI.color = Color.white;
        GUILayout.Label(wdc.WorldData.name,myStyle);

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0f, Screen.height * .2f, 36, 350));

        GUIContent[] brushTypeGUI = editorMode.Value == 1 ? terrainBrushTips.GuiContentArray() : brushTypeTips.GuiContentArray();
        EditorGUI.BeginChangeCheck();
        brushTypeSelected.Value = GUILayout.SelectionGrid(brushTypeSelected.Value, brushTypeGUI, 1);
        if (EditorGUI.EndChangeCheck()) {
            brushSelectionChanged.Event.Invoke();
            Event.current.Use();
        }
        
        GUILayout.EndArea();

        switch (brushTypeSelected.Value)
            {
                case 0:
                    GUILayout.BeginArea(new Rect(40, Screen.height * .2f, 36, 40));

                EditorGUI.BeginChangeCheck();
                    useCircleVoxelBrush.Value = GUILayout.Toggle(useCircleVoxelBrush.Value, voxelOptions.GuiContentArray()[1], "Button");
                if (EditorGUI.EndChangeCheck()) {
                    EditorUtility.SetDirty(useCircleVoxelBrush);
                }
                GUILayout.EndArea();
                    GUILayout.BeginArea(new Rect(40, Screen.height * .27f, 36, 300));
                EditorGUI.BeginChangeCheck();

                brushSize.Value = (int)GUILayout.VerticalSlider(brushSize.Value, 1, 16, GUILayout.Height(96));
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(brushSize);
                }
                GUILayout.EndArea();

                    break;
                case 4:
                    GUILayout.BeginArea(new Rect(40, Screen.height * .2f, 36, 350));
                    GUILayout.BeginVertical();
                    fillConditions.Checktop = GUILayout.Toggle(fillConditions.Checktop, fillTips.Value[0].guiElement(), "Button");
                    fillConditions.CheckBottom = GUILayout.Toggle(fillConditions.CheckBottom, fillTips.Value[1].guiElement(), "Button");
                    fillConditions.CheckHeight = GUILayout.Toggle(fillConditions.CheckHeight, fillTips.Value[2].guiElement(), "Button");
                    fillConditions.CheckRotation = GUILayout.Toggle(fillConditions.CheckRotation, fillTips.Value[3].guiElement(), "Button");
                    fillConditions.CheckWalkable = GUILayout.Toggle(fillConditions.CheckWalkable, fillTips.Value[4].guiElement(), "Button");
                    fillConditions.CheckTint = GUILayout.Toggle(fillConditions.CheckTint, fillTips.Value[5].guiElement(), "Button");

                    GUILayout.EndVertical();
                    GUILayout.EndArea();
                break;
            }
        //}
        GUI.Label(new Rect(10, Screen.height - 165, 100, 100), "LMB: Act");
        GUI.Label(new Rect(10, Screen.height - 150, 100, 100), "RMB: Opposite");
        GUI.Label(new Rect(10, Screen.height - 135, 135, 100), "+/-: Change Brush Size");
        GUI.Label(new Rect(10, Screen.height - 120, 200, 100), "Alt-click: Deselect Everything");
        GUI.Label(new Rect(10, Screen.height - 105, 200, 100), "Shift: Navegation");

        //GUI.Label(new Rect(10, Screen.height - 105, 100, 100), "RMB: Opposite");


        Handles.EndGUI();
        //Event e = Event.current;
        //if (e != null)
        //{
        //    // FocusType.Passive means that it is forbidden to accept control focus
        //    int controlID = GUIUtility.GetControlID(FocusType.Passive);
        //    if (e.type == EventType.Layout)
        //    {
        //        HandleUtility.AddDefaultControl(controlID);
        //    }
        //}

    }
    //GUIContent[] getEditorGuiContent(bool tile, bool terrain) {
    //     GUIContent[] editModeGUI;

    //    if (terrain) 
    //         editModeGUI = tile ? editorModeTips.GuiContentArray() : new GUIContent[] { editorModeTips.GuiContentArray()[1] };
    //    else
    //            editModeGUI = terrain? editorModeTips.GuiContentArray() : new GUIContent[] { editorModeTips.GuiContentArray()[0] };

    //        return editModeGUI;
    //}

    
    private void OnDisable()
    {
        Clean();
    }
  
    private void OnDestroy()
    {
        Clean();
    }
    void Clean()
    {
        //SceneView.duringSceneGui -= OnScene;
        terrainVisibilityChanged.Event.RemoveListener(OnTerraiVisibilityChanged);
        chunckChanged.Event.RemoveListener(OnCunckChanged);
    }
}
