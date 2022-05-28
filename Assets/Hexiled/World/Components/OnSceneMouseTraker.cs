using UnityEngine;
using Hexiled.World.SO;
namespace Hexiled.World.Components
{
    public class OnSceneMouseTraker : MonoBehaviour
    {
        [SerializeField]
        //[HideInInspector]
        Vector3SO markerPos;
        [SerializeField]
        //[HideInInspector]
        IntSO currentLayer;
        [SerializeField]
        //[HideInInspector]
        Vector3SO mouseHitPosition;
        [SerializeField]
        //[HideInInspector]
        IntSO brushSize;
        [SerializeField]
        //[HideInInspector]
        Vector2IntSO currentChunck;

        [SerializeField]
        //[HideInInspector]
        BoolSO UseCircleVoxelBrush;

        //[SerializeField]
        //[HideInInspector]
        //OperationsState opState;

        [SerializeField]
        //[HideInInspector]
        IntSO brushTypeSelected;
        [SerializeField]
        //[HideInInspector]
        WorldDataContainer wdc;
        [SerializeField]
        GeneratorGraph graph;
        private void OnEnable()
        {
            //Editor Default Resources
            //if (markerPos == null)
            //    markerPos = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables + "MarkerPos" + ".asset");
            //if (currentLayer == null)
            //    currentLayer = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "CurrentLayer" + ".asset");

            //if (mouseHitPosition == null)
            //    mouseHitPosition = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables + "MouseHitPosition" + ".asset");
            //if (brushSize == null)
            //    brushSize = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "BrushSize" + ".asset");
            //if (currentChunck == null)
            //    currentChunck = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables + "SelectedChunk" + ".asset");
            //if (UseCircleVoxelBrush == null)
            //    UseCircleVoxelBrush = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "UseCircleBrush" + ".asset");
            //if (opState == null)
            //    opState = (OperationsState)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "Operations State" + ".asset");
            //if (brushTypeSelected == null)
            //    brushTypeSelected = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "BrushTypeSelect" + ".asset");
            //if (wdc == null)
            //    wdc = (WorldDataContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "World Data Container" + ".asset");

        }
        private void OnDrawGizmosSelected()
        {
            //if (markerPos == null)
            //    markerPos = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables + "MarkerPos" + ".asset");
            //if (currentLayer == null)
            //    currentLayer = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "CurrentLayer" + ".asset");

            //if (mouseHitPosition == null)
            //    mouseHitPosition = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables + "MouseHitPosition" + ".asset");
            //if (brushSize == null)
            //    brushSize = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "BrushSize" + ".asset");
            //if (currentChunck == null)
            //    currentChunck = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables + "SelectedChunk" + ".asset");
            //if (UseCircleVoxelBrush == null)
            //    UseCircleVoxelBrush = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "UseCircleBrush" + ".asset");
            //if (opState == null)
            //    opState = (OperationsState)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "Operations State" + ".asset");
            //if (brushTypeSelected == null)
            //    brushTypeSelected = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "BrushTypeSelect" + ".asset");
            //if (wdc == null)
            //    wdc = (WorldDataContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "World Data Container" + ".asset");

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(markerPos.Value, Vector3.one * 1.1f);

            var position = new Vector3Int(Mathf.FloorToInt(currentChunck.Value.x), 0, Mathf.FloorToInt(currentChunck.Value.y)) * WorldHelpers.MapSize;

            Gizmos.color = Color.green;

            switch (brushTypeSelected.Value)
            {
                case 0:
                    VoxelOp(position);
                    break;
                case 1:
                    ShowTempselected(position);
                    break;
                case 2:
                    ShowTempselected(position);
                    break;
                case 3:
                    ShowTempselected(position);
                    break;
                case 5:
                    ShowPattern();
                    break;
            }

            //if (opState.SceneSelectedTiles.Count > 0)
            //{
            //    Gizmos.color = Color.blue;
            //    foreach (Vector3Int v in opState.SceneSelectedTiles)
            //    {
            //        Gizmos.DrawWireCube(v + .8f * Vector3.up, new Vector3(1, 1, 1));
            //    }
            //}
        }
        void ShowTempselected(Vector3Int position)
        {
            //if (opState.TempSelectedTiles.Count > 0)
            //{
            //    Gizmos.color = Color.yellow;
            //    foreach (Vector3Int v in opState.TempSelectedTiles)
            //    {
            //        Gizmos.DrawWireCube(position + v + Vector3.one - Vector3.up, new Vector3(1, 1, 1) * 1.1f);
            //    }
            //}
        }
        void VoxelOp(Vector3Int position)
        {
            int brushextend = brushSize.Value - 1;
            for (int i = -brushextend; i < brushextend + 1; i++)
            {
                for (int j = -brushextend; j < brushextend + 1; j++)
                {
                    float _x = i + markerPos.Value.x;
                    float _z = j + markerPos.Value.z;
                    if (_x >= position.x && _x < WorldHelpers.MapSize + position.x && _z >= position.z && _z < WorldHelpers.MapSize + position.z)
                    {

                        if (UseCircleVoxelBrush.Value)
                        {
                            if (new Vector2(i, j).SqrMagnitude() < brushextend * brushextend + .1f)
                                Gizmos.DrawWireCube(markerPos.Value + new Vector3(i, 0, j), Vector3.one);
                        }
                        else
                        {
                            Gizmos.DrawWireCube(markerPos.Value + new Vector3(i, 0, j), Vector3.one);
                        }
                    }
                }
            }
        }

        void ShowPattern()
        {
            //if (wdc.WorldData.MapPrefabsContainer.MapPrefabs.Length == 0) return;
            //Gizmos.color = Color.cyan;
            //MapPrefab mp = wdc.WorldData.MapPrefabsContainer.MapPrefabs[opState.selectedMapPrefab];
            //if (mp == null) return;
            //Vector3 position = currentChunck.Value * WorldHelpers.MapSize;
            //for (int j = 0; j < mp.mapHeight; j++)
            //{
            //    for (int k = 0; k < mp.mapSizeY + 1; k++)
            //    {
            //        for (int i = 0; i < mp.mapSizeX + 1; i++)
            //        {
            //            if (mp[i, j, k].tileType != TileType.Empty)
            //                if (i + markerPos.Value.x >= position.x &&
            //                    i + markerPos.Value.x - position.x < WorldHelpers.MapSize &&
            //                    k + markerPos.Value.z >= position.z &&
            //                    k + markerPos.Value.z - position.z < WorldHelpers.MapSize)

            //                    Gizmos.DrawWireCube(markerPos.Value + new Vector3(i, j, k), new Vector3(1, 1, 1));

            //        }
            //    }
            //}
        }
    }
}
