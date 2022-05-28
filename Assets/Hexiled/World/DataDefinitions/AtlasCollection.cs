using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "AtlasCollection", menuName = "Hexiled/Yeti3D/Containers/AtlasCollection")]
    public class AtlasCollection : ScriptableObject
    {
        [SerializeField]
        TileMap[] tileMaps;
        [SerializeField]
        UnityEvent TileMapsChanged;

        public TileMap[] TileMaps { get => tileMaps; set => tileMaps = value; }

        private void OnEnable()
        {
            if (TileMapsChanged == null)
                TileMapsChanged = new UnityEvent();

        }

        public void Add(TileMap tm) {
            List<TileMap> _tms = new List<TileMap>(tileMaps);
            _tms.Add(tm);
            tileMaps = _tms.ToArray();
        }
        public string[] GetNames()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < tileMaps.Length; i++)
            {
                names.Add(tileMaps[i].Texture.name);
            }
            return names.ToArray();
        }
        public Material[] GetMaterials() {
            Material[] materials = new Material[tileMaps.Length];
            for (int i = 0; i < tileMaps.Length; i++)
            {
                materials[i] = tileMaps[i].Material;
            }
            return materials;
        }
        void OnValidate()
        {
            if (TileMapsChanged == null)
                TileMapsChanged = new UnityEvent();

            TileMapsChanged.Invoke();
        }
    }
    [System.Serializable]
    public class TileMap
    {
        [SerializeField]
        Material material;
        //[SerializeField]
        //Texture texture;
        [SerializeField]
        int rows;
        [SerializeField]
        int columns;

        public string name { get => material.mainTexture.name; }
        public int Rows { get => rows;  set => rows = value; }
        public int Columns { get => columns;  set => columns = value; }
        public Texture Texture { get => material.mainTexture; set => material.mainTexture = value; }
        public Material Material { get => material; set => material = value; }
    }
}