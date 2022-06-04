using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(menuName = "Hexiled/Editor Specific/Operations State")]
    public class OperationsState : ScriptableObject
    {
        public Dictionary<Vector3Int, float> selectedTerrainTilesDict = new Dictionary<Vector3Int, float>();

        [SerializeField]
        IntSO brushSize;
        [SerializeField]
        BoolSO useCircleVoxelBrush;

        //public List<Vector3Int> TempSelectedTiles;
        public List<Vector3Int> SceneSelectedTiles;
        public List<bool> layerVisibility = new List<bool>();

        public bool setTint, paintTopTexture, paintBottomTexture, changeLight, darken, makeWalkable, walkableValue, changeHeight,
            rotateUV, rndDark, setDarknes, preserveRotation, rndRot, setRotation,
            rndHeight, setHeight, autoChangeLayerWithSize, useBottomLayerAsMask, meshRotate, switchLayerUp, switchLayerDown,
            autoDeleteWhenTooThin, useTextureCoordinates, useTint;
        public int setHeightValue, combineVSinstanciate;
        public float setDarknessValue, minVal = 0.3f, maxVal = 0.6f;

        public int setRotationValue;
        public int room;
        public int selectedMesh, meshRot, steep, meshPrefabSelection;
        public int selectedColor;
        public int roomIndex;
        [SerializeField]
        BoolSO useSelectionAsMask;
        [SerializeField]
        IntSO toolBar;
        [SerializeField]
        BoolSO usePlacedMask;
        [SerializeField]
        IntSO editingLayer;
        [SerializeField]
        IntSO currentRoom;

        public bool UseSelectionAsMask { get => useSelectionAsMask.Value; set => useSelectionAsMask.Value = value; }
        public int ToolBar { get => toolBar.Value; set => toolBar.Value = value; }
        public bool UsePlacedMask { get => usePlacedMask.Value; set => usePlacedMask.Value = value; }
        public int EditingLayer { get => editingLayer.Value; set => editingLayer.Value = value; }
        //public Vector3Variable SelectedChunk { get => selectedChunk; set => selectedChunk = value; }
        public IntSO BrushSize { get => brushSize; set => brushSize = value; }
        public BoolSO UseCircleVoxelBrush { get => useCircleVoxelBrush; set => useCircleVoxelBrush = value; }

        //public int CurrentRoom { get => currentRoom.Value; set => currentRoom.Value = value; }

        //public bool editTile;
        public string _filename;
        //public bool UseWorldEditor;
        public int selectedMapPrefab;
        public int prefabRot;
        public int BrushTypeSelected;
        public bool CircleBrush;

        //private void OnEnable()
        //{
        //    if (toolBar == null)
        //        toolBar = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "ToolBar" + ".asset");
        //    if (brushSize == null)
        //        brushSize = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "BrushSize" + ".asset");
        //    if (useCircleVoxelBrush == null)
        //        useCircleVoxelBrush = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "UseCircleBrush" + ".asset");
        //    if (useSelectionAsMask == null)
        //        useSelectionAsMask = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "UseSelectionAs Mask" + ".asset");
        //    if (usePlacedMask == null)
        //        usePlacedMask = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "UseplacedMask" + ".asset");
        //    if (editingLayer == null)
        //        editingLayer = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "CurrentLayer" + ".asset");

        //}
    }
}