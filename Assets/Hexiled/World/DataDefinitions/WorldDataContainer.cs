using UnityEngine;
using UnityEditor;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "WorldDataContainer",menuName = "Hexiled /Editor Specific/WorldDataContainer")]
    public class WorldDataContainer : ScriptableObject
    {
        [SerializeField]
        //[HideInInspector]
        WorldData worldData;
        
        
        public WorldData WorldData { get => worldData; set => worldData = value; }

        private void OnEnable()
        {
            //if (worldData == null) { 
            //    worldData = (WorldData)EditorGUIUtility.LoadRequired(InternalPaths.worldData + "Default_worldData" + ".asset");
                Load(worldData);
            //}
        }
        void Load(WorldData worldData)
        {
            var path = AssetDatabase.GetAssetPath(worldData);
            WorldDataContainer wdc = this;
            Object[] data = AssetDatabase.LoadAllAssetsAtPath(path);

            foreach (Object o in data)
            {
                if (o is WorldData)
                {
                    WorldData wd = o as WorldData;
                    wdc.WorldData = wd;
                }
            }
            foreach (Object o in data)
            {

                if (o is TerrainSettings)
                {
                    TerrainSettings terrainSettings = o as TerrainSettings;
                    wdc.WorldData.TerrainSettings = terrainSettings;
                }
                if (o is AtlasCollection)
                {
                AtlasCollection worldTilemaps = o as AtlasCollection;
                    wdc.WorldData.WorldTilemaps = worldTilemaps;
                }
                if (o is WorldMeshes)
                {
                    WorldMeshes worldMeshes = o as WorldMeshes;
                    wdc.WorldData.WorldMeshes = worldMeshes;
                }
                if (o is MapPrefabsContainer)
                {
                    MapPrefabsContainer mapPrefabs = o as MapPrefabsContainer;
                    wdc.WorldData.MapPrefabsContainer = mapPrefabs;
                }
                Undo.RegisterCompleteObjectUndo(this, "World Data Changed");
                //EditorUtility.SetDirty(this);
            }
        }
    }
}
