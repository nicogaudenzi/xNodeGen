using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName ="TileEditorStateHolder",menuName = "Hexiled/Editor Specific/TileEditorStateHolder")]
    public class TileEditorWindowStateHolder : ScriptableObject
    {
        //[SerializeField]
        IntSO selectedTilemap;
        //[SerializeField]
        IntSO textureTileHeight;
        //[SerializeField]
        IntSO textureTileWidth;
        [SerializeField]
        List<Vector2Int> selectedTiles;
        [SerializeField]
        List<Vector2Int> botSelectedTiles;

        public int SelectedTilemap { get => selectedTilemap.Value; set => selectedTilemap.Value = value; }
        //public int TextureTileHeight { get => textureTileHeight.Value; set => textureTileHeight.Value = value; }
        //public int TextureTileWidth { get => textureTileWidth.Value; set => textureTileWidth.Value = value; }
        public List<Vector2Int> SelectedTiles { get => selectedTiles; set => selectedTiles = value; }
        public List<Vector2Int> BotSelectedTiles { get => botSelectedTiles; set => botSelectedTiles = value; }

       
    }
}