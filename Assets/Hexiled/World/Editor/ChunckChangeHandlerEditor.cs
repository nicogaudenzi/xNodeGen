using UnityEngine;
using UnityEditor;
using Hexiled.World.Events;
using Hexiled.World.Components;
using Hexiled.World.SO;
using Hexiled.World;

[CustomEditor(typeof(ChunckChangeHandler))]
[InitializeOnLoad]
public class ChunckChangeHandlerEditor : Editor
{
    [SerializeField]
    WorldDataContainer wdc;
    //[SerializeField]
    //OperationsState opState;
    [SerializeField]
    Vector3SO markerPos;

    [SerializeField]
    Vector3Int mouseDragStart;
    [SerializeField]
    Vector3SO mouseHitPosition;

    [SerializeField]
    VoidEventSO finishedEdition, askTileRepaint, askMeshRepaint, startMouseDrag, resolveClick;//, selectedChunckChanged;
    [SerializeField]
    Vector3EventSO selectedChunckChanged;
    [SerializeField]
    Vector2IntSO selectedChunk;
    [SerializeField]
    IntSO terrainOps;
    [SerializeField]
    Vector2IntEventSO askForTerrainRepaint;

    bool changedChunk;
    Vector2Int chunkCoords;
    bool needsTileRepaint;
    private void OnEnable()
    {
        if (Application.isPlaying) return;
        SceneView.duringSceneGui += DuringScene;
    }

    private void OnDisable() => Clean();

    private void OnDestroy() => Clean();

    void Clean()
    {
        if (Application.isPlaying) return;
        SceneView.duringSceneGui -= DuringScene;
    }


    void DuringScene(SceneView sceneView)
    {
        Event current = Event.current;

        if (current.modifiers == EventModifiers.CapsLock || current.modifiers == EventModifiers.Shift)
        {
            return;
        }

        switch (current.type)
        {
            case EventType.MouseDown:
                changedChunk = ChangedChunk(current);
                if (!changedChunk)
                    startMouseDrag.Event?.Invoke();
                break;

            case EventType.MouseDrag:
                if (!changedChunk)
                    resolveClick.Event?.Invoke();
                current.Use();
                break;
            case EventType.MouseUp:
                if (!changedChunk)
                {
                    bool remove = RemoveChunk(current);
                    if (!remove)
                        resolveClick.Event?.Invoke();
                }
                break;
        }

        if (needsTileRepaint)
        {
            finishedEdition.Event?.Invoke();
            askTileRepaint.Event?.Invoke();
            needsTileRepaint = false;
        }

    }

    bool ChangedChunk(Event e)
    {
        //if (EventSystem.current.IsPointerOverGameObject()) {
        //    Debug.Log("Pointer over gameobject");
        //    return false;
        //}
        Ray ray = Camera.current.ScreenPointToRay(Event.current.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
         //   Debug.Log(hit);
        }
        chunkCoords = new Vector2Int(Mathf.FloorToInt(markerPos.Value.x / WorldHelpers.MapSize), Mathf.FloorToInt(markerPos.Value.z / WorldHelpers.MapSize));

        if (e.button == 0)
        {
            if (selectedChunk.Value != new Vector2(chunkCoords.x, chunkCoords.y))
            {
                selectedChunk.Value = new Vector2Int(chunkCoords.x, chunkCoords.y);

                selectedChunckChanged.Event.Invoke(new Vector3(chunkCoords.x,0,chunkCoords.y));
                //opState.SceneSelectedTiles.Clear();
                return true;
            }
        }
        return false;
    }

    bool RemoveChunk(Event e)
    {
        if (terrainOps.Value != 5) return false;
        if (e.button == 1)
        {
            chunkCoords = new Vector2Int(Mathf.FloorToInt(markerPos.Value.x / WorldHelpers.MapSize), Mathf.FloorToInt(markerPos.Value.z / WorldHelpers.MapSize));
            wdc.WorldData.DeleteTerrainKey(chunkCoords);
            askForTerrainRepaint.Event.Invoke(chunkCoords);
            EditorUtility.SetDirty(wdc.WorldData);
            return true;
        }
        //if (e.button == 1)
        //{
        //    chunkCoords = new Vector2Int(Mathf.FloorToInt(markerPos.Value.x / WorldHelpers.MapSize), Mathf.FloorToInt(markerPos.Value.z / WorldHelpers.MapSize));

        //    if ((wdc.WorldData.WorldChuncks.ContainsKey(chunkCoords) || wdc.WorldData.TerrainChunkHolder.ContainsKey(chunkCoords)) && e.type == EventType.MouseUp)
        //    {
        //        Vector2 offset = new Vector2(chunkCoords.x - selectedChunk.Value.x, chunkCoords.y - selectedChunk.Value.y);
        //        if (offset != Vector2.zero)
        //        {

        //            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Remove Chunk");
        //            int option = EditorUtility.DisplayDialogComplex("Remove Chunk",
        //    "Which Data do you want to  Delete?",
        //    "Terrain",
        //    "Tilemap",
        //    "Cancel");

        //            switch (option)
        //            {
        //                // Terrain.
        //                case 0:
        //                    if (wdc.WorldData.TerrainChunkHolder.ContainsKey(chunkCoords))
        //                        wdc.WorldData.TerrainChunkHolder.Remove(chunkCoords);

        //                    break;

        //                // Tilemap.
        //                case 1:
        //                    if (wdc.WorldData.WorldChuncks.ContainsKey(chunkCoords))
        //                        wdc.WorldData.RemoveKey(chunkCoords);
        //                    break;

        //                // Cancel.
        //                case 2:
        //                    break;

        //                default:
        //                    Debug.LogError("Unrecognized option.");
        //                    break;
        //            }
        //            EditorUtility.SetDirty(wdc.WorldData);
        //            AssetDatabase.SaveAssets();
        //            return true;
        //        }
        //    }
        //}
        return false;
    }

   

}

