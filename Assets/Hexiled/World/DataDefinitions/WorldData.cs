using UnityEngine;
using Hexiled.World.Events;
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
    
    [CreateAssetMenu(menuName = "Hexiled/World/WorldData")]
    public class WorldData: ScriptableObject
    {
        [SerializeField]
        ChunkHolder worldChuncks;
        [SerializeField]
        TerrainChunkHolder terrainChunkHolder;
        [SerializeField]
        TerrainNoiseHoler terrains;
        [SerializeField]
        MeshesHolder mesheHolder;
        [SerializeField]
        ChunkHeightHolder tileHeightHolder;
        [SerializeField]
        Texture2D palette;
        [SerializeField]
        AtlasCollection worldTilemaps;
        [SerializeField]
        WorldMeshes worldMeshes;
        [SerializeField]
        TerrainSettings terrainSettings;
        [SerializeField]
        MapPrefabsContainer mapPrefabsContainer;

        public Material PropertiesMaterial { get; private set; }

        public WorldData(Material propertiesMaterial) => PropertiesMaterial = propertiesMaterial;

        public ChunkHolder WorldChuncks { get => worldChuncks; private set => worldChuncks = value; }
        public TerrainChunkHolder TerrainChunkHolder { get => terrainChunkHolder; set { terrainChunkHolder = value;
#if UNITY_EDITOR
                Debug.Log("Setting Dirty from TerrainChunckHolder Setter");
                EditorUtility.SetDirty(this);
#endif
            }
        }
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
        [SerializeField]
        VoidEventSO worldDataChanged;
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
            if (terrainChunkHolder == null)
            {
                terrainChunkHolder = new TerrainChunkHolder() {
                    { Vector2Int.zero,new SerializableMultiArray<TerrainTileData>()}
                };
            }
            
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
            EditorUtility.SetDirty(this);
               worldDataChanged.Event.Invoke();
            UnityEditor.EditorApplication.delayCall -= MyOnValidate;
        }
#endif
    }
    
}
