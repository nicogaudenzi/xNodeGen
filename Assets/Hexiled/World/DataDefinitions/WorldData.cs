using UnityEngine;
using UnityEngine.Events;
//using UnityAtoms.BaseAtoms;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEditor;
using Hexiled.World.Data;
namespace Hexiled.World.SO
{
    [System.Serializable]
    public class ChunkHolder : SerializableDictionaryBase<Vector2, SerializableMultiArray<TileData>> { }
    [System.Serializable]
    public class ChunkHeightHolder : SerializableDictionaryBase<Vector2,float> { }

    [System.Serializable]
    public class TerrainChunkHolder : SerializableDictionaryBase<Vector2, SerializableMultiArray<TerrainTileData>> { }
    [System.Serializable]
    public class TerrainNoiseHoler : SerializableDictionaryBase<Vector2, SerializableMultiArray<float>> { }
    [System.Serializable]
    public class MeshesHolder : SerializableDictionaryBase<Vector3, int> { }
    
    [CreateAssetMenu(menuName = "Hexiled/Yeti3D/Containers/WorldData")]
    public class WorldData: ScriptableObject
    {
        [SerializeField]
        //[HideInInspector]
        ChunkHolder worldChuncks;
        [SerializeField]
        //[HideInInspector]
        TerrainChunkHolder terrainChunkHolder;
        [SerializeField]
        //[HideInInspector]
        TerrainNoiseHoler terrains;
        [SerializeField]
        //[HideInInspector]
        MeshesHolder mesheHolder;
        [SerializeField]
        //[HideInInspector]
        ChunkHeightHolder tileHeightHolder;
        [SerializeField]
        //[HideInInspector]
        Texture2D palette;
        //[SerializeField]
        //[HideInInspector]
        public AtlasCollection worldTilemaps;
        [SerializeField]
        //[HideInInspector]
        WorldMeshes worldMeshes;
        [SerializeField]
        //[HideInInspector]
        TerrainSettings terrainSettings;
        [SerializeField]
        //[HideInInspector]
        MapPrefabsContainer mapPrefabsContainer;

        public Material PropertiesMaterial { get; private set; }

        public WorldData(Material propertiesMaterial) => PropertiesMaterial = propertiesMaterial;

        public ChunkHolder WorldChuncks { get => worldChuncks; private set => worldChuncks = value; }
        public TerrainChunkHolder TerrainChunkHolder { get => terrainChunkHolder;  set => terrainChunkHolder = value; }
        public TerrainNoiseHoler Terrains { get => terrains; set => terrains = value; }
        public TerrainSettings TerrainSettings { get=>terrainSettings; set=>terrainSettings=value; }
        public AtlasCollection WorldTilemaps { get=> worldTilemaps; set=>worldTilemaps=value; }

        public Texture2D Palette { get=>palette; set=>palette=value; }
        public GameObject[] Meshes { get => WorldMeshes.Meshes; set => WorldMeshes.Meshes = value; }
        public WorldMeshes WorldMeshes { get=>worldMeshes; set=>worldMeshes=value; }
        public MeshesHolder MesheHolder { get => mesheHolder; set => mesheHolder = value; }
        public MapPrefabsContainer MapPrefabsContainer { get=> mapPrefabsContainer; set=> mapPrefabsContainer=value; }
        public ChunkHeightHolder TileHeightHolder { get => tileHeightHolder;}
        public WorldEventSystem WorldEventSystem { get => worldEventSystem; set => worldEventSystem = value; }

        //public VoidEvent WorldDataChanged { get; set; }

        [HideInInspector]
        public Texture2D PreviewIcon;

        [SerializeField]
        WorldEventSystem worldEventSystem;

        void OnEnable()
        {
            if (worldChuncks == null)
                worldChuncks = new ChunkHolder()
                {
                    { Vector2Int.zero,new SerializableMultiArray<TileData>() }
                };

            if (terrains == null)
            {
                terrains = new TerrainNoiseHoler() {
                    { Vector2Int.zero,new SerializableMultiArray<float>()}
                };
            }
            //if (TerrainSettings == null)
            //{
            //    TerrainSettings = (TerrainSettings)EditorGUIUtility.LoadRequired(InternalPaths.terrainSettings+"DefaultTerrianSettings"+".asset");
            //}
            //if (WorldTilemaps == null)
            //{
            //    WorldTilemaps = (WorldTilemaps)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+"DefaultWorldTileMaps"+".asset");
            //}
            //if (PropertiesMaterial == null)
            //{
            //    PropertiesMaterial = (Material)EditorGUIUtility.LoadRequired(InternalPaths.materials+"DefaultTerrainMaterial"+".mat");
            //}
            //if (Palette == null)
            //{
            //    Palette = (Texture2D)EditorGUIUtility.LoadRequired(InternalPaths.textures+"DefaultPalette"+".png");
            //}
            //if (WorldMeshes == null)
            //{
            //    WorldMeshes = (WorldMeshes)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+"World Mesh Container"+".asset");
            //}
            //if (MesheHolder == null)
            //{
            //    MesheHolder = new MeshesHolder();
            //}
            //if (MapPrefabsContainer == null)
            //{
            //    MapPrefabsContainer = (MapPrefabsContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+"MapPrefabsContainer"+".asset");
            //}
            //if (terrainChunkHolder == null)
            //{
            //    terrainChunkHolder = new TerrainChunkHolder() {
            //        { Vector2Int.zero,new SerializableMultiArray<TerrainTileData>() }
            //    };
            //}
            //if (WorldDataChanged == null)
            //{
            //    WorldDataChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents+"worldDataChanged"+".asset");
            //}
            if (tileHeightHolder == null)
            {
                tileHeightHolder = new ChunkHeightHolder()
                {
                    { Vector2Int.zero, 0}
                };
            }

        }

        public void RemoveKey(Vector2Int key) {
            worldChuncks.Remove(key);
            tileHeightHolder.Remove(key);
        }
        public void AddBothKey(Vector2Int key) {
            AddTileKey(key);
            //AddTerrainKey(key);
        }
        public void AddTileKey(Vector2Int key)
        {
            if (!worldChuncks.ContainsKey(key))
            {
                worldChuncks.Add(key, new SerializableMultiArray<TileData>());
                tileHeightHolder.Add(key, 0);
            }
        }
        public void AddTerrainKey(Vector2Int key)
        {
            if (!terrainChunkHolder.ContainsKey(key))
                terrainChunkHolder.Add(key, new SerializableMultiArray<TerrainTileData>());
        }
        public void DeleteTileKey(Vector2Int key)
        {
            if (worldChuncks.ContainsKey(key))
                worldChuncks.Remove(key);
        }
        public void DeleteTerrainKey(Vector2Int key)
        {
            if (terrainChunkHolder.ContainsKey(key))
                terrainChunkHolder.Remove(key);
        }
        public void ResetData() {
            worldChuncks = new ChunkHolder()
                {
                    { Vector2Int.zero,new SerializableMultiArray<TileData>() }
                };
        }
        public void ClearTerrainData(Vector3 v) {
            Terrains[v.Fomchunck3ToChunck2Int()] = null;
            TerrainChunkHolder[v.Fomchunck3ToChunck2Int()] = new SerializableMultiArray<TerrainTileData>();
        }
#if UNITY_EDITOR
        void OnValidate() { UnityEditor.EditorApplication.delayCall += MyOnValidate; }
        void MyOnValidate()
        {
               WorldEventSystem.worldDataChanged?.Invoke();
            UnityEditor.EditorApplication.delayCall -= MyOnValidate;
        }
#endif
    }
    
}
