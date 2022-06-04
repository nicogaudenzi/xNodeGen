using System;
using System.Collections.Generic;
using Hexiled.World;
using Hexiled.World.Components;
using Hexiled.World.Data;
using Hexiled.World.SO;
using Hexiled.World.Events;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(WorldEventHandler))]
public class WorldEventHandlerEditor : Editor
{
    [SerializeField]
    WorldDataContainer wdc;
    [SerializeField]
    OperationsState opState;
    [SerializeField]
    Vector3SO markerPos;
    [SerializeField]
    IntSO BrushSize, selectedPalette, brushTypeSelected, editingLayer,
                TileMapSelected;
    [SerializeField]
    IntSO textureTileWidth, textureTileHeight;
    [SerializeField]
    IntSO Toolbar;
    [SerializeField]
    IntSO editorMode;
    [SerializeField]
    Vector3Int mouseDragStart;
    [SerializeField]
    Vector3SO mouseHitPosition;

    [SerializeField]
    BoolSO hit, terrainVisibility;
    [SerializeField]
    VoidEventSO finishedEdition, askTileRepaint, brushLoaded, layerUndo, startMouseDrag, resolveClick,
        meshRotate, instanceBaked, askMeshRepaint, bakedFinished;
    [SerializeField]
    IntEventSO editorModeChanged;

    [SerializeField]
    BoolSO UseCircleVoxelBrush;
    [SerializeField]
    TileEditorWindowStateHolder tileEditorState;
    [SerializeField]
    FillConditionsCheck fcc;

    //[SerializeField]
    //FloatSO minDarkRnd, maxDarkRnd;


    [SerializeField]
    FloatEventSO setHeight, putPrefab;
    [SerializeField]
    Vector3EventSO deleteMesh, placePrefab;
    [SerializeField]
    Vector2IntSO selectedChunk;

    System.Random random;
    bool needsTileRepaint;
    List<Vector2> globalChecked;
    WorldEventHandler weh;
    //SerializedObject so;
    private void OnEnable()
    {


        weh = (WorldEventHandler)target;
       
        resolveClick.Event.AddListener(ResolveClick);
        startMouseDrag.Event.AddListener(StartMouseDrag);
        bakedFinished.Event.AddListener(OnBakeFinished);

        if (Application.isPlaying) return;
        Selection.selectionChanged += OnSelectionChanged;
        globalChecked = new List<Vector2>();
        SceneView.duringSceneGui += DuringScene;
        if (random == null)
        {
            random = new System.Random();
        }
    }

    private void OnDisable() => Clean();

    private void OnDestroy() => Clean();

    void Clean()
    {
        SceneView.duringSceneGui -= DuringScene;
        Selection.selectionChanged -= OnSelectionChanged;
        resolveClick.Event.RemoveListener(ResolveClick);
        startMouseDrag.Event.RemoveListener(StartMouseDrag);
        bakedFinished.Event.RemoveListener(OnBakeFinished);

    }

    void OnBakeFinished()
    {
        AssetDatabase.SaveAssets();
    }
    void OnSelectionChanged()
    {
        int oldValue = editorMode.Value;
        if (Selection.activeGameObject == weh.gameObject)
        {
            editorMode.Value = 0;
            if (oldValue != 0)
                editorModeChanged.Event.Invoke(editorMode.Value);
        }
    }


    public void DuringScene(SceneView sceneView)
    {
        if (Event.current.type == EventType.ValidateCommand)
        {

            switch (Event.current.commandName)
            {

                case "UndoRedoPerformed":
                    Debug.Log("Undoing: " + Event.current.commandName);

                    layerUndo.Event.Invoke();
                    brushLoaded.Event.Invoke();
                    askTileRepaint.Event.Invoke();
                    askMeshRepaint.Event.Invoke();
                    //Event.current.Use();
                    break;

            }
        }

        if (needsTileRepaint)
        {
            finishedEdition.Event.Invoke();
            askTileRepaint.Event.Invoke();
            needsTileRepaint = false;
            EditorUtility.SetDirty(wdc.WorldData);
        }
        //if (meshesNeedRepaint) {
        //    //setHeight.Event.Invoke(1 + t.height);
        //}
    }

    void StartMouseDrag()
    {
        Event current = Event.current;

        mouseDragStart = markerPos.Value.FloorToInt();

        if (current.modifiers == EventModifiers.Alt)
        {
            if (Toolbar.Value == 3)
                opState.SceneSelectedTiles.Clear();
        }
        current.Use();
    }

    void ResolveClick()
    {
        if (Application.isPlaying) return;
        Event current = Event.current;
        Vector3Int _v = markerPos.Value.FloorToInt() - new Vector3Int(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize;
        int markerPosX = _v.x;
        int markerPosY = _v.y;
        int markerPosZ = _v.z;

        int brushextend = opState.BrushSize.Value - 1;
        Vector2Int v = selectedChunk.Value;
        //if (current.button == 0 && !wdc.WorldData.WorldChuncks.ContainsKey(v))
        //{
        //    wdc.WorldData.AddTileKey(v);
        //}

        Undo.RegisterCompleteObjectUndo(wdc.WorldData, "TileMap Change");
        Vector3Int adjSelectedChunck = new Vector3Int(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize;
        Vector3Int adjMouseDragStart = mouseDragStart - adjSelectedChunck;

        switch (brushTypeSelected.Value)
        {
            case 0:

                for (int i = -brushextend; i < brushextend + 1; i++)
                {
                    for (int j = -brushextend; j < brushextend + 1; j++)
                    {
                        Vector3Int tilePos = new Vector3Int(markerPosX + i, markerPosY, markerPosZ + j);
                        bool insideMap = tilePos.x >= 0 && tilePos.x < WorldHelpers.MapSize && tilePos.z >= 0 && tilePos.z < WorldHelpers.MapSize;

                        if (insideMap)
                        {
                            if (UseCircleVoxelBrush.Value)
                            {
                                if (new Vector2(i, j).SqrMagnitude() < brushextend * brushextend + .1f)
                                {

                                    if (current.button == 1)
                                    {
                                        Erase(tilePos.x, tilePos.y, tilePos.z, selectedChunk.Value);
                                    }
                                    else if (current.button == 0)
                                    {
                                        Attach(tilePos.x, tilePos.y, tilePos.z, selectedChunk.Value);
                                    }
                                }
                            }
                            else
                            {
                                if (current.button == 0)
                                {

                                    Attach(tilePos.x, tilePos.y, tilePos.z, selectedChunk.Value);
                                }
                                else
                                {
                                    Erase(tilePos.x, tilePos.y, tilePos.z, selectedChunk.Value);
                                }
                            }
                        }
                    }
                }
                break;
            case 1:
                if (current.type != EventType.MouseDrag)
                {
                    if (current.button == 1)
                        ShapeErase();
                    else if (current.button == 0)
                        ShapeAttach();
                }
                else
                {
                    opState.selectedTerrainTilesDict.Clear();
                    adjSelectedChunck = new Vector3Int(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize;
                    Vector3Int currPos = new Vector3Int(markerPosX, editingLayer.Value, markerPosZ);
                    adjMouseDragStart = mouseDragStart - adjSelectedChunck;

                    int radiusSqr = Mathf.FloorToInt(Vector3.Magnitude(currPos - adjMouseDragStart));

                    for (int i = adjMouseDragStart.x - radiusSqr; i < adjMouseDragStart.x + radiusSqr + 1; i++)
                    {
                        for (int j = adjMouseDragStart.z - radiusSqr; j < adjMouseDragStart.z + radiusSqr + 1; j++)
                        {
                            Vector3Int tilePos = new Vector3Int(i, opState.EditingLayer, j);
                            bool insideMap = tilePos.x + 1 >= 0 && tilePos.x + 1 < WorldHelpers.MapSize && tilePos.z + 1 >= 0 && tilePos.z + 1 < WorldHelpers.MapSize;
                            if (Vector3.Magnitude(adjMouseDragStart - tilePos) < radiusSqr + .1f && insideMap)
                                opState.selectedTerrainTilesDict.Add(tilePos + adjSelectedChunck + new Vector3Int(-1, 0, -1), 0);
                        }
                    }
                }
                break;
            case 2:

                if (current.type != EventType.MouseDrag)
                {
                    if (current.button == 1)
                        ShapeErase();
                    else if (current.button == 0)
                        ShapeAttach();
                }

                else
                {
                    adjSelectedChunck = new Vector3Int(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize;
                    adjMouseDragStart = mouseDragStart - adjSelectedChunck;
                    opState.selectedTerrainTilesDict.Clear();
                    int startX = markerPosX > adjMouseDragStart.x ? adjMouseDragStart.x : markerPosX;
                    int startY = markerPosZ > adjMouseDragStart.z ? adjMouseDragStart.z : markerPosZ;

                    int endX = markerPosX > adjMouseDragStart.x ? markerPosX : adjMouseDragStart.x;
                    int endY = markerPosZ > adjMouseDragStart.z ? markerPosZ : adjMouseDragStart.z;

                    for (int i = startX; i < endX; i++)
                    {
                        for (int j = startY + 1; j < endY + 1; j++)
                        {
                            Vector3Int tilePos = new Vector3Int(i, markerPosY, j);
                            bool insideMap = tilePos.x + 1 >= 0 && tilePos.x + 1 < WorldHelpers.MapSize && tilePos.z + 1 >= 0 && tilePos.z + 1 < WorldHelpers.MapSize;
                            if (insideMap)
                                opState.selectedTerrainTilesDict.Add(new Vector3Int(i, markerPosY, j) + adjSelectedChunck + new Vector3Int(-1, 0, -1), 0);
                        }
                    }
                }
                break;
            case 3:

                if (current.type != EventType.MouseDrag)
                {
                    if (current.button == 1)
                        ShapeErase();
                    else if (current.button == 0)
                        ShapeAttach();
                }
                else
                {

                    opState.selectedTerrainTilesDict.Clear();
                    int startX = markerPosX > adjMouseDragStart.x ? adjMouseDragStart.x : markerPosX;
                    int startY = markerPosZ > adjMouseDragStart.z ? adjMouseDragStart.z : markerPosZ;


                    int distX = markerPosX - adjMouseDragStart.x;
                    int distY = markerPosZ - adjMouseDragStart.z;
                    int dist = Mathf.FloorToInt(Vector3.Magnitude(new Vector3(distX, 0, distY)));
                    for (int i = 0; i < dist; i++)
                    {
                        int _x = startX;
                        int _y = startY;
                        if (dist != 0)
                        {
                            _x = adjMouseDragStart.x + Mathf.FloorToInt(i * distX / (float)dist);
                            _y = adjMouseDragStart.z + Mathf.FloorToInt(i * distY / (float)dist);
                        }

                        Vector3Int tilePos = new Vector3Int(_x, markerPosY, _y);
                        //bool insideMap = tilePos.x >= 0 && tilePos.x < WorldHelpers.MapSize && tilePos.z + 1 >= 0 && tilePos.z + 1 < WorldHelpers.MapSize;
                        //    if (insideMap && !opState.selectedTerrainTilesDict.Keys.Contains(tilePos))
                            opState.selectedTerrainTilesDict.Add(tilePos + adjSelectedChunck + new Vector3Int(-1, 0, -1),0);
                    }
                }
                break;
            case 4:
                TileData t = wdc.WorldData.WorldChuncks[v][_v.x, _v.y, _v.z];

                if (current.button == 1 && current.type != EventType.MouseDrag)
                {
                    if (IsMouseOnLayer())
                    {
                        FillDelete(markerPosX, markerPosY, markerPosZ, v, t.UVs, t.bottomUVs);
                    }
                    globalChecked.Clear();
                }
                else if (current.button == 0 && current.type != EventType.MouseDrag)
                {
                    if (Toolbar.Value == 2)
                    {
                        Debug.LogWarning("You shouldnt fill with meshes");
                        return;
                    }
                    if (t.tileType == TileType.Empty)
                    {
                        FillAttach(markerPosX, markerPosY, markerPosZ, v);
                    }
                    else
                    {
                        FillPaint(markerPosX, markerPosY, markerPosZ, v, t.UVs, t.bottomUVs);
                    }
                    globalChecked.Clear();

                }
                break;
            case 5:
                if (wdc.WorldData.MapPrefabsContainer.MapPrefabs.Length == 0) return;
                MapPrefab mp = wdc.WorldData.MapPrefabsContainer.MapPrefabs[opState.selectedMapPrefab];
                Dictionary<GameObject, int> tempMeshes = new Dictionary<GameObject, int>();
                for (int j = 0; j < mp.mapHeight; j++)
                {
                    for (int k = 0; k < mp.mapSizeY + 1; k++)
                    {
                        for (int i = 0; i < mp.mapSizeX + 1; i++)
                        {
                            if (current.button == 0)
                            {
                                TileData mpt = mp[i, j, k];
                                if (mpt.tileType != TileType.Empty)
                                {
                                    Material[] currentMaterials = wdc.WorldData.WorldTilemaps.GetMaterials();
                                    Material topMat = mp.Tilemaps.TileMaps[mpt.topIndex].Material;
                                    int topMatIndex = Array.IndexOf(currentMaterials, topMat);

                                    if (topMatIndex == -1)
                                    {
                                        wdc.WorldData.WorldTilemaps.Add(mp.Tilemaps.TileMaps[mpt.topIndex]);
                                        topMatIndex = mp.Tilemaps.TileMaps.Length;
                                    }


                                    //if (i + markerPosX < WorldHelpers.MapSize && k + markerPosZ < WorldHelpers.MapSize)
                                    //{
                                    TileData original = wdc.WorldData.WorldChuncks[selectedChunk.Value][markerPosX + i, markerPosY + j, markerPosZ + k];
                                    if (original.tileType == TileType.Mesh && original.meshType == MeshType.Prefab)
                                    {
                                        deleteMesh.Event.Invoke(new Vector3(markerPosX + i, markerPosY + j, markerPosZ + k));
                                    }
                                    wdc.WorldData.WorldChuncks[selectedChunk.Value][markerPosX + i, markerPosY + j, markerPosZ + k] = new TileData(mp[i, j, k])
                                    {
                                        topIndex = topMatIndex,
                                    };

                                    if (mpt.tileType == TileType.Mesh && mpt.meshType == MeshType.Prefab)
                                    {
                                        int meshIndex = Array.IndexOf(wdc.WorldData.Meshes, mp.Meshes[mpt.meshPrefabIndex]);
                                        if (meshIndex == -1)
                                        {
                                            if (!tempMeshes.ContainsKey(mp.Meshes[mpt.meshPrefabIndex]))
                                            {
                                                tempMeshes.Add(mp.Meshes[mpt.meshPrefabIndex], tempMeshes.Count);
                                                //    List<GameObject> _meshes = new List<GameObject>(wdc.WorldData.Meshes)
                                                //{
                                                //    mp.Meshes[mpt.meshPrefabIndex]
                                                //};
                                                //    wdc.WorldData.Meshes = _meshes.ToArray();
                                                meshIndex = tempMeshes.Count - 1;
                                            }
                                            else
                                            {
                                                meshIndex = tempMeshes[mp.Meshes[mpt.meshPrefabIndex]];
                                            }
                                        }


                                        wdc.WorldData.WorldChuncks[selectedChunk.Value][markerPosX + i, markerPosY + j, markerPosZ + k].meshPrefabIndex = meshIndex;
                                        wdc.WorldData.MesheHolder.Add(new Vector3(markerPosX + i, markerPosY + j, markerPosZ + k) + new Vector3(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize, meshIndex);
                                        placePrefab.Event.Invoke(new Vector3(markerPosX + i, markerPosY + j, markerPosZ + k) + new Vector3(selectedChunk.Value.x, 0, selectedChunk.Value.y) * WorldHelpers.MapSize);
                                    }
                                    //}
                                }

                            }
                            else
                            {
                                _ = mp[i, j, k];

                                Vector3Int adj = new Vector3Int(markerPosX + i, markerPosY + j, markerPosZ + k);
                                ErasePaint(adj.x, adj.y, adj.z, v);
                            }

                        }
                    }
                }
                //needsTileRepaint = true;
                break;
        }
        needsTileRepaint = true;

    }

    public void Attach(int _x, int _y, int _z, Vector2Int v)
    {
        if (wdc.WorldData.WorldChuncks.ContainsKey(v))
        {
            TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];

            if (opState.UsePlacedMask)
                if (t.tileType != TileType.Ground)
                    return;

            if (opState.UseSelectionAsMask && opState.ToolBar != 3)
                if (!opState.SceneSelectedTiles.Contains(new Vector3Int(_x, _y, _z)))
                    return;
        }
        switch (Toolbar.Value)
        {
            case 0:
                AddPaint(_x, _y, _z, v);
                break;
            case 1:
                ChangeHeight(_x, _y, _z, v);
                break;
            case 2:
                Geometries(_x, _y, _z, v);

                break;
            case 3:
                TileSelection(_x, _z, v);

                break;

        }
        // needsTileRepaint = true;

    }

    public void Erase(int _x, int _y, int _z, Vector2Int v)
    {
        if (opState.UseSelectionAsMask && Toolbar.Value != 3)
            if (!opState.SceneSelectedTiles.Contains(new Vector3Int(_x, _y, _z)))
                return;

        switch (Toolbar.Value)
        {
            case 0:
                ErasePaint(_x, _y, _z, v);
                break;
            case 1:
                EraseHeight(_x, _y, _z, v);
                break;
            case 2:
                EraseMeshes(_x, _y, _z, v);
                break;
            case 3:
                Vector2Int[] chunckinfo = new Vector2Int[] { v, new Vector2Int(_x, _z) };
                Vector2Int _pos = WorldHelpers.GetWorldPosFromChunkInfo(chunckinfo);
                Vector3Int pos = new Vector3Int(_pos.x, 0, _pos.y);
                if (opState.SceneSelectedTiles.Contains(pos))
                {
                    opState.SceneSelectedTiles.Remove(pos);

                }
                break;

        }
        //needsTileRepaint = true;

    }
    void ShapeAttach()
    {
        foreach (var v in opState.selectedTerrainTilesDict.Keys)
        {
            Attach(v.x + 1, v.y, v.z + 1, selectedChunk.Value);
        }
        mouseDragStart = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        opState.selectedTerrainTilesDict.Clear();
    }

    void ShapeErase()
    {
        foreach (var v in opState.selectedTerrainTilesDict.Keys)
        {
            Erase(v.x + 1, v.y, v.z + 1, selectedChunk.Value);
        }
        mouseDragStart = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        opState.selectedTerrainTilesDict.Clear();
    }

    void FillAttach(int _x, int _y, int _z, Vector2Int v)
    {
        if (globalChecked.Contains(new Vector2(_x, _z))) return;
        globalChecked.Add(new Vector2(_x, _z));
        Attach(_x, _y, _z, v);
        if (!fcc.Checktop && !fcc.CheckBottom && !fcc.CheckRotation && !fcc.CheckHeight && !fcc.CheckWalkable && !fcc.CheckTint) return;
        for (int i = 0; i < 4; i++)
        {
            TileData n = wdc.WorldData.WorldChuncks[v].GetNeighbor(_x, _y, _z, (Direction)i);

            if (n == null || n.tileType == TileType.Empty)
            {
                Vector2Int nPos = wdc.WorldData.WorldChuncks[v].GetNeighborCoords(_x, _y, _z, (Direction)i);
                if (nPos.x == int.MaxValue && nPos.y == int.MaxValue) continue;
                if (nPos.x >= 0 && nPos.x <= WorldHelpers.MapSize && nPos.y >= 0 && nPos.y <= WorldHelpers.MapSize)
                    FillAttach(nPos.x, _y, nPos.y, v);
            }
        }
    }

    void FillPaint(int _x, int _y, int _z, Vector2Int v, Vector2[] referenceUVs, Vector2[] bottomReferenceUVs)
    {
        if (globalChecked.Contains(new Vector2(_x, _z))) return;
        globalChecked.Add(new Vector2(_x, _z));
        float referenceHeight = wdc.WorldData.WorldChuncks[v][_x, _y, _z].height;
        Color referenceColor = wdc.WorldData.WorldChuncks[v][_x, _y, _z].color;
        Attach(_x, _y, _z, v);

        for (int i = 0; i < 4; i++)
        {
            TileData n = wdc.WorldData.WorldChuncks[v].GetNeighbor(_x, _y, _z, (Direction)i);
            if (n != null && n.tileType != TileType.Empty)
            {
                if (FillCompare(wdc.WorldData.WorldChuncks[v][_x, _y, _z], n, referenceUVs, bottomReferenceUVs, referenceHeight, referenceColor))
                {
                    Vector2Int nPos = wdc.WorldData.WorldChuncks[v].GetNeighborCoords(_x, _y, _z, (Direction)i);
                    if (nPos.x == int.MaxValue && nPos.y == int.MaxValue) continue;
                    if (nPos.x >= 0 && nPos.x <= WorldHelpers.MapSize && nPos.y >= 0 && nPos.y <= WorldHelpers.MapSize)
                        FillPaint(nPos.x, _y, nPos.y, v, referenceUVs, bottomReferenceUVs);
                }
            }
        }
    }

    void FillDelete(int _x, int _y, int _z, Vector2Int v, Vector2[] referenceUVs, Vector2[] bottomReferenceUVs)
    {
        if (globalChecked.Contains(new Vector2(_x, _z))) return;
        globalChecked.Add(new Vector2(_x, _z));
        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];
        float referenceHeight = t.height;
        Color referenceColor = wdc.WorldData.WorldChuncks[v][_x, _y, _z].color;

        Erase(_x, _y, _z, v);

        for (int i = 0; i < 4; i++)
        {
            TileData n = wdc.WorldData.WorldChuncks[v].GetNeighbor(_x, _y, _z, (Direction)i);
            if (n == null) continue;
            if (n.tileType != TileType.Empty)
            {
                if (FillCompare(wdc.WorldData.WorldChuncks[v][_x, _y, _z], n, referenceUVs, bottomReferenceUVs, referenceHeight, referenceColor))
                {
                    Vector2Int nPos = wdc.WorldData.WorldChuncks[v].GetNeighborCoords(_x, _y, _z, (Direction)i);
                    FillDelete(nPos.x, _y, nPos.y, v, referenceUVs, bottomReferenceUVs);
                }
            }
        }
    }

    bool FillCompare(TileData t, TileData n, Vector2[] referenceUVs, Vector2[] bottomReferenceUVs, float referenceHeight, Color referenceColor)
    {
        if (!fcc.Checktop && !fcc.CheckBottom && !fcc.CheckRotation && !fcc.CheckHeight && !fcc.CheckWalkable && !fcc.CheckTint) return false;
        bool equal = true;
        if (fcc.Checktop) { equal = equal && WorldHelpers.CompareTopCenter(referenceUVs, n.UVs); }
        if (!equal) return false;
        if (fcc.CheckBottom) { equal = equal && WorldHelpers.CompareUVS(bottomReferenceUVs, n.bottomUVs); }
        if (!equal) return false;

        if (fcc.CheckHeight) { equal = equal && Math.Abs(referenceHeight - n.height) < .001f; }
        if (!equal) return false;

        if (fcc.CheckRotation) { equal = equal && WorldHelpers.CompareUVS(referenceUVs, n.UVs); }
        if (!equal) return false;

        if (fcc.CheckWalkable) { equal = equal && t.walkable == n.walkable; }
        if (!equal) return false;

        if (fcc.CheckTint) { equal = equal && referenceColor == n.color; }
        if (!equal) return false;

        return equal;
    }

    void AddPaint(int _x, int _y, int _z, Vector2Int v)
    {

        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];

        if (t.tileType == TileType.Empty)
        {
            if (opState.useBottomLayerAsMask)
            {
                if (opState.EditingLayer == 0)
                    return;
                if (wdc.WorldData.WorldChuncks[v][_x, _y - 1, _z].tileType == TileType.Empty)
                    return;
            }
        }
        if (opState.useBottomLayerAsMask && opState.EditingLayer != 0)
        {
            if (wdc.WorldData.WorldChuncks[v][_x, _y - 1, _z].tileType != TileType.Empty)
            {
                wdc.WorldData.WorldChuncks[v][_x, _y, _z] = new TileData
                {
                    tileType = TileType.Ground,
                    color = wdc.WorldData.Palette.GetPixel(selectedPalette.Value, 0)
                };

            }

        }

        t.tileType = TileType.Ground;
        if (opState.makeWalkable)
            t.walkable = opState.walkableValue;
        if (opState.changeLight)
        {
            if (opState.darken)
                t.color *= .9f;
            else
                t.color *= 1.1f;
        }
        if (opState.setTint)
            t.color = wdc.WorldData.Palette.GetPixel(selectedPalette.Value, 0);

        if (opState.rndDark)
        {
            float rndRage = opState.maxVal - opState.minVal;
            var c = (float)random.NextDouble() * rndRage + opState.minVal;
            t.color = new Color(c, c, c, 1);
        }
        if (opState.setDarknes)
        {
            var c = opState.setDarknessValue;
            t.color = new Color(c, c, c, 1);
        }
        if (opState.setHeight)
        {
            switch (opState.setHeightValue)
            {
                case 0:
                    t.height = 0;
                    break;
                case 1:
                    t.height = -.25f;
                    break;
                case 2:
                    t.height = -.5f;
                    break;
                case 3:
                    t.height = -.75f;
                    break;
                case 4:
                    t.height = -1f;
                    break;
            }
        }

        SetUVS(t);

        //if (opState.changeRoom)
        //{
        //    t.room = opState.CurrentRoom;
        //}
        if (opState.rndRot)
        {
            if (random == null)
            {
                random = new System.Random();
            }
            int rotation = random.Next(0, 4);
            for (int i = 0; i < rotation; i++)
            {
                Vector2[] _uvs = new Vector2[4];
                _uvs[0] = t.UVs[1];
                _uvs[1] = t.UVs[2];
                _uvs[2] = t.UVs[3];
                _uvs[3] = t.UVs[0];
                t.rotation++;
                if (t.rotation == 4)
                {
                    t.rotation = 0;
                }
                t.SetTopUVs(_uvs);
                if (t.tileType == TileType.Mesh)
                {
                    int j = (int)t.meshRot + 1;
                    t.meshRot = (MeshRot)(j % 4);
                }
            }
        }

        if (opState.setRotation)
        {
            int rotation = opState.setRotationValue;
            for (int i = 0; i < rotation; i++)
            {
                Vector2[] _uvs = new Vector2[4];
                _uvs[0] = t.UVs[1];
                _uvs[1] = t.UVs[2];
                _uvs[2] = t.UVs[3];
                _uvs[3] = t.UVs[0];
                t.rotation++;
                if (t.rotation == 4)
                {
                    t.rotation = 0;
                }
                t.SetTopUVs(_uvs);
                if (t.tileType == TileType.Mesh)
                {
                    int j = (int)t.meshRot + 1;
                    t.meshRot = (MeshRot)(j % 4);
                }
            }
        }
        if (opState.rndHeight)
        {
            int h = random.Next(0, 4);
            t.height = (.25f * h) - 1;
        }
        if (opState.setHeight)
            t.height = GetHeightFromSelection();

        wdc.WorldData.WorldChuncks[v][_x, _y, _z] = t;
        //needsTileRepaint = true;
    }

    void ErasePaint(int _x, int _y, int _z, Vector2Int v)
    {
        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];
        if (t.tileType == TileType.Mesh)
        {
            if (t.meshType == MeshType.Prefab)
                deleteMesh.Event.Invoke(new Vector3(_x, _y, _z) + new Vector3(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize);

            t.meshType = MeshType.Ramp;
            t.tileType = TileType.Ground;
        }
        else
        {
            wdc.WorldData.WorldChuncks[v][_x, _y, _z] = new TileData
            {
                tileType = TileType.Empty
            };
            //needsTileRepaint = true;
        }
        Vector3Int _v = new Vector3Int(_x, _y, _z);
        if (wdc.WorldData.MesheHolder.ContainsKey(_v))
        {
            wdc.WorldData.MesheHolder.Remove(_v);
        }
    }

    void EraseHeight(int _x, int _y, int _z, Vector2Int v)
    {
        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];
        if (opState.changeHeight && t.height + 1 > 0)
        {
            t.height -= .25f;
            if (t.tileType == TileType.Mesh && t.meshType == MeshType.Prefab)
                setHeight.Event.Invoke(1 + t.height);
        }
        else
        {
            if (opState.autoDeleteWhenTooThin && opState.changeHeight)
            {
                t.tileType = TileType.Empty;
                t.rotation = 0;
                t.height = 0;
                t.room = -1;
                t.walkable = false;
                t.meshType = MeshType.Ramp;
            }
        }
        if (opState.rotateUV)
        {
            Vector2[] _uvs = new Vector2[4];
            _uvs[0] = t.UVs[3];
            _uvs[1] = t.UVs[0];
            _uvs[2] = t.UVs[1];
            _uvs[3] = t.UVs[2];
            t.rotation--;
            if (t.rotation == -1)
            {
                t.rotation = 3;
            }
            t.SetTopUVs(_uvs);
            if (t.tileType == TileType.Mesh)
            {
                int i = (int)t.meshRot - 1;
                i = i == -1 ? 3 : i;
                t.meshRot = (MeshRot)(i % 4);
            }
        }
    }
    void EraseMeshes(int _x, int _y, int _z, Vector2Int v)
    {
        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];
        if (t.tileType == TileType.Mesh)
        {
            wdc.WorldData.WorldChuncks[v][_x, _y, _z].tileType = TileType.Ground;

            if (t.meshType == MeshType.Prefab)
            {
                deleteMesh.Event.Invoke(new Vector3(_x,_y, _z) + new Vector3(selectedChunk.Value.x,0,selectedChunk.Value.y) * WorldHelpers.MapSize);
            }
        }
        else
        {
            t.tileType = TileType.Empty;
            t.rotation = 0;
            t.height = 0;
            t.room = -1;
            t.walkable = false;
            t.meshType = MeshType.Ramp;
        }
    }

    void ChangeHeight(int _x, int _y, int _z, Vector2Int v)
    {
        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];
        SerializableMultiArray<TileData> chunck = wdc.WorldData.WorldChuncks[v];
        if (t.tileType != TileType.Empty)
        {

            if (opState.changeHeight && t.height + 1 < 1)
            {
                t.height += .25f;
                if (t.tileType == TileType.Mesh && t.meshType == MeshType.Prefab)
                    setHeight.Event.Invoke(1 + t.height);

            }
            else if (editingLayer.Value + 1 < chunck.MapHeight && opState.autoChangeLayerWithSize)
            {
                chunck[_x, _y + 1, _z].height = 0;
                chunck[_x, _y + 1, _z].tileType = t.tileType;
                chunck[_x, _y + 1, _z].height = -.75f;
                chunck[_x, _y + 1, _z].topIndex = tileEditorState.SelectedTilemap;
                chunck[_x, _y + 1, _z].botIndex = tileEditorState.SelectedTilemap;

                SetUVS(chunck[_x, _y + 1, _z]);
                editingLayer.Value++;
            }

            if (opState.rotateUV)
            {
                Vector2[] _uvs = new Vector2[4];
                _uvs[0] = t.UVs[1];
                _uvs[1] = t.UVs[2];
                _uvs[2] = t.UVs[3];
                _uvs[3] = t.UVs[0];
                t.rotation++;
                if (t.rotation == 4)
                {
                    t.rotation = 0;
                }
                t.SetTopUVs(_uvs);
                if (t.tileType == TileType.Mesh)
                {
                    int i = (int)t.meshRot + 1;
                    t.meshRot = (MeshRot)(i % 4);
                }
            }

            SetUVS(t);

        }
        else
        {
            if (opState.useBottomLayerAsMask && editingLayer.Value > 0)
            {
                if (chunck[_y - 1, _z, _x].height >= 0)
                {
                    if (opState.changeHeight)
                    {
                        t.tileType = TileType.Ground;
                        t.height = -1f;
                        t.topIndex = tileEditorState.SelectedTilemap; ;
                        t.botIndex = tileEditorState.SelectedTilemap; ;

                    }
                }
                SetUVS(t);

            }
            else
            {
                if (opState.changeHeight)
                {
                    t.tileType = TileType.Ground;
                    t.height = -1f;
                }
            }
            //SetUVS(t);
        }
    }
    public void SetUVS(TileData t)
    {
        if (opState.paintTopTexture)
        {
            Vector2[] _uvs = new Vector2[4];
            float sizeW = 1 / (float)textureTileWidth.Value;
            float sizeH = 1 / (float)textureTileHeight.Value;
            _uvs[0] = new Vector2(sizeW * (tileEditorState.SelectedTiles[0].x + 1), sizeH * (-tileEditorState.SelectedTiles[0].y - 1)) + new Vector2(0, textureTileHeight.Value);
            _uvs[1] = new Vector2(sizeW * tileEditorState.SelectedTiles[0].x, sizeH * (-tileEditorState.SelectedTiles[0].y - 1)) + new Vector2(0, textureTileHeight.Value);
            _uvs[2] = new Vector2(sizeW * tileEditorState.SelectedTiles[0].x, -sizeH * tileEditorState.SelectedTiles[0].y) + new Vector2(0, textureTileHeight.Value);
            _uvs[3] = new Vector2(sizeW * (tileEditorState.SelectedTiles[0].x + 1), -sizeH * tileEditorState.SelectedTiles[0].y) + new Vector2(0, textureTileHeight.Value);
            if (opState.preserveRotation)
                for (int i = 0; i < t.rotation; i++)
                {
                    _uvs = WorldHelpers.rotateUVS(_uvs);
                }
            t.toptileX = tileEditorState.SelectedTiles[0].x;
            t.toptileY = tileEditorState.SelectedTiles[0].y;
            t.topIndex = tileEditorState.SelectedTilemap;
            t.SetTopUVs(_uvs);
        }
        if (opState.paintBottomTexture)
        {
            Vector2[] _uvs = new Vector2[4];

            float sizeW = 1 / (float)textureTileWidth.Value;
            float sizeH = 1 / (float)textureTileHeight.Value;
            _uvs[0] = new Vector2(sizeW * tileEditorState.BotSelectedTiles[0].x, -sizeH * tileEditorState.BotSelectedTiles[0].y) + new Vector2(0, textureTileHeight.Value);
            _uvs[1] = new Vector2(sizeW * (tileEditorState.BotSelectedTiles[0].x + 1), -sizeH * tileEditorState.BotSelectedTiles[0].y) + new Vector2(0, textureTileHeight.Value);
            _uvs[2] = new Vector2(sizeW * (tileEditorState.BotSelectedTiles[0].x + 1), sizeH * (-tileEditorState.BotSelectedTiles[0].y - 1)) + new Vector2(0, textureTileHeight.Value);
            _uvs[3] = new Vector2(sizeW * tileEditorState.BotSelectedTiles[0].x, sizeH * (-tileEditorState.BotSelectedTiles[0].y - 1)) + new Vector2(0, textureTileHeight.Value);

            t.bottileX = tileEditorState.BotSelectedTiles[0].x;
            t.bottileY = tileEditorState.BotSelectedTiles[0].y;
            t.botIndex = tileEditorState.SelectedTilemap;
            t.SetBottomUVs(_uvs);
        }
    }
    void Geometries(int _x, int _y, int _z, Vector2Int v)
    {
        TileData t = wdc.WorldData.WorldChuncks[v][_x, _y, _z];
        if (t.tileType != TileType.Mesh)
        {
            t.tileType = TileType.Mesh;
            t.meshType = (MeshType)opState.selectedMesh;
            t.meshRot = (MeshRot)opState.meshRot;
            t.meshSteep = (MeshSteep)opState.steep;
            t.meshPrefabIndex = 0;

            if (t.meshType == MeshType.Prefab)
            {
                t.meshPrefabIndex = opState.meshPrefabSelection;

                t.walkable = false;
            }
            else
            {
                SetUVS(t);
            }
            if (opState.selectedMesh == (int)MeshType.Prefab)
            {
                putPrefab.Event.Invoke(1 + t.height);
                return;
            }
        }
        else
        {
            if (opState.meshRotate)
            {
                int rot = (int)t.meshRot;
                t.meshRot = (MeshRot)rot + 1 % 3;
                meshRotate.Event.Invoke();
            }
        }
    }

    void TileSelection(int _x, int _z, Vector2Int v)
    {
        Vector2Int[] chunckinfo = new Vector2Int[] { v, new Vector2Int(_x, _z) };
        Vector2Int _pos = WorldHelpers.GetWorldPosFromChunkInfo(chunckinfo);
        Vector3Int pos = new Vector3Int(_pos.x, 0, _pos.y);
        if (!opState.SceneSelectedTiles.Contains(pos))
            opState.SceneSelectedTiles.Add(pos);
    }

    private bool IsMouseOnLayer()
    {
        return mouseHitPosition.Value.x >= selectedChunk.Value.x * WorldHelpers.MapSize && mouseHitPosition.Value.x < WorldHelpers.MapSize + selectedChunk.Value.x * WorldHelpers.MapSize &&
        mouseHitPosition.Value.z >= selectedChunk.Value.y * WorldHelpers.MapSize && mouseHitPosition.Value.z < WorldHelpers.MapSize + selectedChunk.Value.y * WorldHelpers.MapSize;
    }

    float GetHeightFromSelection()
    {
        float r = 0;
        switch (opState.setHeightValue)
        {
            case 0:
                r = 0;
                break;
            case 1:
                r = -.25f;
                break;
            case 2:
                r = -.5f;
                break;
            case 3:
                r = -.75f;
                break;
            case 4:
                r = -1f;
                break;
        }
        return r;
    }
}