using UnityEngine;
using Hexiled.World.Data;
using UnityEditor;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "MapPrefab", menuName = "Hexiled/Yeti3D/Settings/MapPrefab")]
    public class MapPrefab : ScriptableObject
    {
        public DataLayer<TileData>[] data;
        public int mapSizeX = 8;
        public int mapSizeY = 8;

        public int mapHeight = 1;
        [SerializeField]
        GameObject[] meshes;
        //[SerializeField]
        AtlasCollection tilemaps;

        
        public GameObject[] Meshes { get => meshes; set => meshes = value; }
        public AtlasCollection Tilemaps { get => tilemaps; set => tilemaps = value; }

        //public MeshesHolder prefabs;
        //private void OnEnable()
        //{
        //    if (tilemaps == null)
        //        tilemaps = (WorldTilemaps)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects+"DefaultWorldTileMaps"+".asset");
        //}
        public TileData this[int i, int j, int k]//This is to fix inversion of the indexes;
        {
            get
            {
                return data[j][k][i];
            }
            set
            {
                data[j][k][i] = value;
            }
        }
        
    }
}