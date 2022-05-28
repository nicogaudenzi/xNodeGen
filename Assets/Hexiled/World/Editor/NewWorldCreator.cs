using UnityEngine;
using UnityEditor;
using Hexiled.World.SO;
//using System.IO;
public class NewWorldCreator : ScriptableWizard
{
    public string newWorldName;
    public WorldDataContainer wdc;
    public WorldEventSystem worldEventSystem;
    [MenuItem("Tools/Hexiled/Yeti3D/Create New World")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<NewWorldCreator>("Create New World", "Cancel", "Create");
    }
    
    void OnWizardCreate()
    {
       
    }

    void OnWizardUpdate()
    {
        helpString = "Name Your World";
        //GUILayout.Label("Name Your World", EditorStyles.boldLabel);
        //newWorldName = EditorGUILayout.TextField("Name Your World", newWorldName);
    }

    // When the user presses the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton()
    {
        if (newWorldName == "")
        {
            Debug.LogWarning("Yeti3D World Name can't be empty.");
            this.Close();
            return;
        }
        //WorldDataContainer wdc = AssetDatabase.LoadAssetAtPath<WorldDataContainer>("Assets/Editor Default Resources/" + InternalPaths.stateObjects + "World Data Container" + ".asset");
        //if(wdc.WorldData==null)
        //    wdc.WorldData = AssetDatabase.LoadAssetAtPath<WorldData>("Assets/Editor Default Resources/" + InternalPaths.stateObjects + "DefaultWorld" + ".asset");
        WorldData wd = CreateInstance<WorldData>();
        wd.name = newWorldName;

        TerrainSettings terrainSettings = CreateInstance<TerrainSettings>();
        terrainSettings.name = newWorldName + "_TerrainSettings";
        AtlasCollection worldTilemaps = CreateInstance<AtlasCollection>();
        worldTilemaps.name = newWorldName + "_Tilemaps";
        WorldMeshes worldMeshes = CreateInstance<WorldMeshes>();
        worldMeshes.name = newWorldName + "_Meshes";
        MapPrefabsContainer mapPrefabsContainer = CreateInstance<MapPrefabsContainer>();
        mapPrefabsContainer.name = newWorldName + "_Prefabs";

        wdc.WorldData = wd;
        wdc.WorldData.TerrainSettings = terrainSettings;
        wdc.WorldData.WorldTilemaps = worldTilemaps;
        wdc.WorldData.WorldMeshes = worldMeshes;
        wdc.WorldData.MapPrefabsContainer = mapPrefabsContainer;

        wdc.WorldData.WorldEventSystem = worldEventSystem;

        string wd_path = "Assets/" + newWorldName +"/"+newWorldName+ ".asset";
        string wd_path_terrain = "Assets/" + newWorldName + "/" + newWorldName + "_TerrainSettings.asset";

        AssetDatabase.CreateFolder("Assets", newWorldName);
        AssetDatabase.CreateAsset(wd, wd_path);
        var path = AssetDatabase.GetAssetPath(wd);
        AssetDatabase.CreateAsset(terrainSettings, wd_path_terrain);
        AssetDatabase.AddObjectToAsset(worldTilemaps, path);
        AssetDatabase.AddObjectToAsset(worldMeshes, path);
        AssetDatabase.AddObjectToAsset(mapPrefabsContainer, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.CreateFolder("Assets/"+newWorldName, "BakedMeshes");
       
       
        AssetDatabase.SaveAssets();
        this.Close();
    }
}
