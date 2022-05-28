using UnityEditor;
using Hexiled.World.SO;

public class TerrainEditorWindow : EditorWindow
{
    public WorldEventSystem worldEventSystem;
    //[SerializeField]
    public WorldDataContainer wdc;
    //[SerializeField]
    // VoidEvent TerrainSettingsChanged, worldDataChanged;
    //[SerializeField]
    //BoolEvent AutoUpdateChanged;
    Editor terrainEditor;

    bool autoUpdate = true;
    [MenuItem("Tools/Hexiled/Yeti3D/Terrain Editor")]
    static void Init()
    {
        var window = GetWindow<TerrainEditorWindow>(false, "Terrain Editor");  
        window.ShowUtility();
    }
    private void OnEnable()
    {
        //if (wdc == null)
        //    wdc = (WorldDataContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+"World Data Container"+".asset");

        if (terrainEditor == null)
            terrainEditor = Editor.CreateEditor(wdc.WorldData.TerrainSettings);

        //if (TerrainSettingsChanged == null)
        //    TerrainSettingsChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents+"TerrainUpdate"+".asset");
        //if (worldDataChanged == null)
        //    worldDataChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents+"worldDataChanged"+".asset");
        //if (AutoUpdateChanged == null)
        //    AutoUpdateChanged = (BoolEvent)EditorGUIUtility.LoadRequired(InternalPaths.boolevents+"AutoUpdateChanged"+".asset");

        worldEventSystem.AutoUpdateChanged.Invoke(autoUpdate);
        worldEventSystem.worldDataChanged.AddListener(OnWorldDataChanged);
    }
    public void OnWorldDataChanged() {
        terrainEditor = Editor.CreateEditor(wdc.WorldData.TerrainSettings);
        Repaint();
    }
    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        terrainEditor.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            worldEventSystem.TerrainSettingsChanged.Invoke();
        }
        //EditorGUI.BeginChangeCheck();
        //autoUpdate = GUILayout.Toggle(autoUpdate, "Visualize Neighbours");
        //if (EditorGUI.EndChangeCheck())
        //{
        //    AutoUpdateChanged.Raise(autoUpdate);
        //    TerrainSettingsChanged.Raise();
        //}

    }
    private void OnDestroy()
    {
        if (terrainEditor != null) DestroyImmediate(terrainEditor);
    }
    private void OnDisable()
    {
        if (terrainEditor != null) DestroyImmediate(terrainEditor);
    }
}
