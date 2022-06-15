using UnityEngine;
using UnityEditor;
using Hexiled.World.Components;
using Hexiled.World.SO;
using Hexiled.World.Events;
using Hexiled.World.Data;
using System.Collections.Generic;
namespace Hexiled.World.Editor
{
    [CustomEditor(typeof(TerrainEventHandler))]
    public class TerrainEventHandlerEditor : UnityEditor.Editor
    {
        [SerializeField]
        WorldDataContainer wdc;
        [SerializeField]
        OperationsState opState;
        [SerializeField]
        Vector3SO markerPos;
        [SerializeField]
        IntSO editorMode, terrainOps, brushTypeSelected, editingLayer;
        [SerializeField]
        Vector3Int mouseDragStart;
        [SerializeField]
        Vector3SO mouseHitPosition;
        [SerializeField]
        BoolSO hit, terrainVisibility;
        [SerializeField]
        VoidEventSO finishedEdition,  startMouseDrag, resolveClick;
        [SerializeField]
        Vector2IntEventSO askTerrainRepaint;
        [SerializeField]
        IntEventSO editorModeChanged;
        [SerializeField]
        Vector2IntSO selectedChunk;
        [SerializeField]
        animationCurveSO brushProfile;
        [SerializeField]
        FloatSO brushStength;
        [SerializeField]
        BrushContainer brushContainer;
        [SerializeField]
        GradientSO color;
        TerrainEventHandler teh;
        List<Vector2Int> chunksToRepaint;
        [SerializeField]
        BoolSO useColorWhilePattern;
        [SerializeField]
        BoolSO useGeometryWhilePattern;
        [SerializeField]
        FloatSO heightModifier;
        [SerializeField]
        GeneratorGraph generatorGraph;
        Dictionary<Vector2Int, Generator> generators;
        private void OnEnable()
        {
            chunksToRepaint = new List<Vector2Int>();
            teh = (TerrainEventHandler)this.target;
            generators = new Dictionary<Vector2Int, Generator>();
            if (Application.isPlaying) return;
            SceneView.duringSceneGui += duringScene;
            Selection.selectionChanged += OnSelectionChanged;
            OnSelectionChanged();
            startMouseDrag.Event.AddListener(StartMouseDrag);
            resolveClick.Event.AddListener(ResolveClick);
        }

        private void OnDisable() => Clean();

        private void OnDestroy() => Clean();

        void Clean()
        {
            SceneView.duringSceneGui -= duringScene;
            Selection.selectionChanged -= OnSelectionChanged;
            if (Application.isPlaying) return;
            startMouseDrag.Event.RemoveListener(StartMouseDrag);
            resolveClick.Event.RemoveListener(ResolveClick);

        }
        void OnSelectionChanged()
        {
            int oldValue = editorMode.Value;
            if (Selection.activeGameObject == teh.gameObject ||
                Selection.Contains(teh.gameObject))
            {
                editorMode.Value = 1;
                editingLayer.Value = 0;
                //terrainVisibility.Value = true;
                if (brushTypeSelected.Value > 3) brushTypeSelected.Value = 0;
                finishedEdition.Event.Invoke();
            }
            if (oldValue != 1)
                editorModeChanged.Event.Invoke(editorMode.Value);
        }


        public void duringScene(SceneView sceneView)
        {
            Event current = Event.current;
            if (current.type != EventType.MouseDrag)
            {
                foreach (Vector2Int v in chunksToRepaint)
                {
                    askTerrainRepaint.Event.Invoke(v);

                }
                generators.Clear();
                chunksToRepaint.Clear();
            }
        }

        void StartMouseDrag()
        {
            Event current = Event.current;
            mouseDragStart = markerPos.Value.FloorToInt();
            current.Use();
        }
        void ResolveClick()
        {
            Event current = Event.current;
            Vector3Int _U = new Vector3Int(selectedChunk.Value.x, 0, selectedChunk.Value.y);
            Vector3Int _v = markerPos.Value.FloorToInt() - _U * WorldHelpers.MapSize; ;
            int markerPosX = _v.x;
            int markerPosY = _v.y;
            int markerPosZ = _v.z;

            int brushextend = opState.BrushSize.Value - 1;
            Vector2Int v = selectedChunk.Value;
            if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(v))
            {
                return;
            }
            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "TerrainMap Edition");
            Vector3Int adjSelectedChunck = _U * WorldHelpers.MapSize;
            Vector3Int adjMouseDragStart = mouseDragStart - adjSelectedChunck;

            switch (brushTypeSelected.Value)
            {
                case 0:
                    if (terrainOps.Value == 1)
                    {
                        float totalH = 0;
                        int total = 0;
                        for (int i = -brushextend; i < brushextend + 1; i++)
                        {
                            for (int j = -brushextend; j < brushextend + 1; j++)
                            {
                                float profileDistX = Mathf.Abs(i) / (float)(brushextend);
                                float profileDistY = Mathf.Abs(j) / (float)(brushextend);
                                float strength = new Vector2(profileDistX, profileDistY).magnitude;
                                strength = brushProfile.Value.Evaluate(1 - strength);
                                Vector3Int res = markerPos.Value.FloorToInt() + new Vector3Int(i, 0, j);
                                ChunkInfo ci = res.ChunkInfoFromPos();
                                Vector2Int chunk = ci.chunk;
                                Vector3Int pos = ci.pos;
                                if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                                    wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());
                                totalH += wdc.WorldData.TerrainChunkHolder[chunk][pos.x, pos.y, pos.z].h * strength;
                                total++;
                            }
                        }
                        float mean = totalH / (float)(total);
                        for (int i = -brushextend; i < brushextend + 1; i++)
                        {
                            for (int j = -brushextend; j < brushextend + 1; j++)
                            {
                                Vector3Int res = markerPos.Value.FloorToInt() + new Vector3Int(i, 0, j);
                                ChunkInfo ci = res.ChunkInfoFromPos();
                                Vector2Int chunk = ci.chunk;
                                Vector3Int pos = ci.pos;
                                if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                                    wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());
                                if (opState.UseCircleVoxelBrush.Value)
                                {
                                    if (new Vector2(i, j).SqrMagnitude() < (brushextend) * (brushextend) + .05f)
                                    {

                                        if (current.button == 1)
                                        {

                                            if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                            meanErase(pos.x, pos.y, pos.z, chunk, mean);

                                        }
                                        else if (current.button == 0)
                                        {
                                            if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);

                                            meanAttach(pos.x, pos.y, pos.z, chunk, mean);
                                        }
                                    }
                                }
                                else
                                {
                                    if (current.button == 0)
                                    {
                                        if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                        meanAttach(pos.x, pos.y, pos.z, chunk, mean);
                                    }
                                    else
                                    {
                                        if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                        meanErase(pos.x, pos.y, pos.z, chunk, mean);
                                    }
                                }
                            }
                        }
                    }
                    //Brush Graph
                    else if (terrainOps.Value == 4)
                    {
                        SerializableMultiArray<Color> _colors = brushContainer.Value.GetColors();
                        Generator _height = brushContainer.Value.GetEndGenerator();
                        for (int i = -brushextend; i < brushextend + 1; i++)
                        {
                            for (int j = -brushextend; j < brushextend + 1; j++)
                            {
                                Vector3Int res = markerPos.Value.FloorToInt() + new Vector3Int(i, 0, j);
                                ChunkInfo ci = res.ChunkInfoFromPos();
                                Vector2Int chunk = ci.chunk;
                                Vector3Int pos = ci.pos;
                                float profileDistX = Mathf.Abs(i) / (float)(brushextend);
                                float profileDistY = Mathf.Abs(j) / (float)(brushextend);
                                float strength = new Vector2(profileDistX, profileDistY).magnitude;
                                strength = brushProfile.Value.Evaluate(1 - strength);
                                if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                                    wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());
                                if (opState.UseCircleVoxelBrush.Value)
                                {
                                    if (current.button == 0)
                                    {
                                        if (new Vector2(i, j).SqrMagnitude() < (brushextend) * (brushextend) + .1f)
                                        {
                                            if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                            ApplyGraph(pos.x, pos.y, pos.z, chunk, pos.x, pos.z, _colors, _height.GetValue(pos.x, pos.y, pos.z), strength);
                                        }
                                    }
                                    else
                                    {
                                        if (new Vector2(i, j).SqrMagnitude() < (brushextend) * (brushextend) + .1f)
                                        {
                                            if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                            RemoveGraph(pos.x, pos.y, pos.z, chunk);
                                        }
                                    }
                                }
                                else
                                {
                                    if (current.button == 0)
                                    {
                                        if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                        ApplyGraph(pos.x, pos.y, pos.z, chunk, pos.x, pos.z, _colors, _height.GetValue(pos.x, pos.y, pos.z), strength);
                                    }
                                    else
                                    {
                                        if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);
                                        RemoveGraph(pos.x, pos.y, pos.z, chunk);
                                    }
                                }
                            }
                        }
                    }
                    //Flatten
                    else if (terrainOps.Value == 6)
                    {
                        Vector3Int res = markerPos.Value.FloorToInt();
                        ChunkInfo ci = res.ChunkInfoFromPos();
                        Vector2Int chunk = ci.chunk;
                        Vector3Int pos = ci.pos;
                        
                        if (current.button == 1)
                        { 
                            if(!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))return;
                            if (!generators.ContainsKey(chunk))
                                generators.Add(chunk, generatorGraph.GetUnprocessedNoise(chunk));

                            heightModifier.Value = wdc.WorldData.TerrainChunkHolder[chunk][pos.x,pos.y,pos.z].h+generators[chunk].GetValue(pos.x, pos.y, pos.z);
                            return;
                        }
                        for (int i = -brushextend; i < brushextend + 1; i++)
                        {
                            for (int j = -brushextend; j < brushextend + 1; j++)
                            {
                                Vector3Int _res = markerPos.Value.FloorToInt() + new Vector3Int(i, 0, j);
                                ChunkInfo _ci = _res.ChunkInfoFromPos();
                                Vector2Int _chunk = _ci.chunk;
                                Vector3Int _pos = _ci.pos;
                                if (!generators.ContainsKey(_chunk))
                                    generators.Add(_chunk, generatorGraph.GetUnprocessedNoise(_chunk));
                                if (!chunksToRepaint.Contains(_chunk)) chunksToRepaint.Add(_chunk);
                                if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(_chunk))
                                    wdc.WorldData.TerrainChunkHolder.Add(_chunk, new SerializableMultiArray<TerrainTileData>());
                                if (opState.UseCircleVoxelBrush.Value)
                                {
                                    if (new Vector2(i, j).SqrMagnitude() < (brushextend) * (brushextend) + .1f)
                                    {
                                        if (current.button == 0)
                                        {
                                            wdc.WorldData.TerrainChunkHolder[_chunk][_pos.x, _pos.y, _pos.z].h = heightModifier.Value -generators[_chunk].GetValue(_pos.x , _pos.y, _pos.z );
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    if (current.button == 0)
                                    {
                                        wdc.WorldData.TerrainChunkHolder[_chunk][_pos.x, _pos.y, _pos.z].h = heightModifier.Value - generators[_chunk].GetValue(_pos.x, _pos.y, _pos.z); ;
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        for (int i = -brushextend; i < brushextend + 1; i++)
                        {
                            for (int j = -brushextend; j < brushextend + 1; j++)
                            {
                                Vector3Int res = markerPos.Value.FloorToInt() + new Vector3Int(i, 0, j);
                                ChunkInfo ci = res.ChunkInfoFromPos();
                                Vector2Int chunk = ci.chunk;
                                Vector3Int pos = ci.pos;
                                if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                                    wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());

                                float profileDistX = Mathf.Abs(i) / (float)(brushextend);
                                float profileDistY = Mathf.Abs(j) / (float)(brushextend);
                                float strength = new Vector2(profileDistX, profileDistY).magnitude;
                                strength = brushProfile.Value.Evaluate(1 - strength);
                                if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);

                                if (opState.UseCircleVoxelBrush.Value)
                                {
                                    if (new Vector2(i, j).SqrMagnitude() < (brushextend) * (brushextend) + .1f)
                                    {

                                        if (current.button == 1)
                                        {

                                            terrainErase(pos.x, pos.y, pos.z, chunk);
                                        }
                                        else if (current.button == 0)
                                        {

                                            terrainAttach(pos.x, pos.y, pos.z, chunk, strength);
                                        }
                                    }
                                }
                                else
                                {
                                    if (current.button == 0)
                                    {
                                        terrainAttach(pos.x, pos.y, pos.z, chunk, strength);
                                    }
                                    else
                                    {
                                        terrainErase(pos.x, pos.y, pos.z, chunk);
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
                            ShapeErase(current);
                        else if (current.button == 0)
                            ShapeAttach(current);
                    }
                    else
                    {
                        opState.selectedTerrainTilesDict.Clear();
                        Vector3Int currPos = new Vector3Int(markerPosX, 0, markerPosZ);
                        adjMouseDragStart = mouseDragStart - adjSelectedChunck;

                        int radiusSqr = Mathf.FloorToInt(Vector3.Magnitude(currPos - adjMouseDragStart));

                        for (int i = adjMouseDragStart.x - radiusSqr; i < adjMouseDragStart.x + radiusSqr + 1; i++)
                        {
                            for (int j = adjMouseDragStart.z - radiusSqr; j < adjMouseDragStart.z + radiusSqr + 1; j++)
                            {
                                Vector3Int tilePos = new Vector3Int(i, opState.EditingLayer, j);

                                if (Vector3.Magnitude(adjMouseDragStart - tilePos) < radiusSqr + .1f)
                                    opState.selectedTerrainTilesDict.Add(tilePos+adjSelectedChunck+new Vector3Int(-1,0,-1),0);
                            }
                        }
                    }

                    break;
                case 2:

                    if (current.type != EventType.MouseDrag)
                    {
                        if (current.button == 1)
                            ShapeErase(current);
                        else if (current.button == 0)
                            ShapeAttach(current);
                    }
                    else
                    {
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
                                opState.selectedTerrainTilesDict.Add(new Vector3Int(i, markerPosY, j) + adjSelectedChunck+new Vector3Int(-1,0,-1),0);
                            }
                        }
                    }
                    break;
                case 3:

                    if (current.type != EventType.MouseDrag)
                    {
                        if (current.button == 1)
                            ShapeErase(current);
                        else if (current.button == 0)
                            ShapeAttach(current);
                    }
                    else
                    {

                        opState.selectedTerrainTilesDict.Clear();
                        int startX = markerPosX > adjMouseDragStart.x ? adjMouseDragStart.x : markerPosX;
                        int startY = markerPosZ > adjMouseDragStart.z ? adjMouseDragStart.z : markerPosZ;
                        startX = markerPosX;
                        startY = markerPosZ;

                        int distX = markerPosX - adjMouseDragStart.x;
                        int distY = markerPosZ - adjMouseDragStart.z;
                        int dist = Mathf.FloorToInt(Vector3.Magnitude(new Vector3(distX, 0, distY)));
                        for (int i = 0; i < dist; i++)
                        {
                            int _x = startX;
                            int _y = startY;
                            if (dist != 0)
                            {
                                _x = adjMouseDragStart.x + Mathf.FloorToInt(i * distX / (float)dist)-1;
                                _y = adjMouseDragStart.z + Mathf.FloorToInt(i * distY / (float)dist)-1;
                            }

                            Vector3Int tilePos = new Vector3Int(_x, markerPosY, _y);
                                opState.selectedTerrainTilesDict.Add(tilePos + adjSelectedChunck,0);
                        }
                    }
                    break;
            }
            EditorUtility.SetDirty(wdc.WorldData);
        }
        public void terrainAttach(int _x, int _y, int _z, Vector2Int v,float strength)
        {
            switch (terrainOps.Value)
            {
                case 0:
                    wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h += .01f* brushStength.Value*strength;
                    break;
                case 2:
                    wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h = 0;
                    break;
                case 3:
                    ChangeColor(_x, _y, _z, v, strength);
                    break;
                case 5:
                    wdc.WorldData.AddTerrainKey(v);
                    break;
            }

        }
        void ApplyGraph(int _x, int _y, int _z, Vector2Int v, int i, int j,SerializableMultiArray<Color> _colors, float  _height, float strength)
        {
            if(useGeometryWhilePattern.Value)
                wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h = _height;
            if (useColorWhilePattern.Value)
            {
                Color c = _colors[i, 0, j];
                float factor = 1 - strength * brushStength.Value / 2f;
                c.a = factor;
                wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].Color = c;
            }
        }
        void RemoveGraph(int _x, int _y, int _z, Vector2Int v)
        {
            if (useGeometryWhilePattern.Value)
                wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h = 0;
            if (useColorWhilePattern.Value)
            {
                wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].Color = Color.black;
                wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].Color.a = 0;
            }
        }
        void ChangeColor(int _x, int _y, int _z, Vector2Int v, float strength)
        {
            Color c = color.Value.Evaluate(strength); 
            float factor = 1-strength * brushStength.Value / 2f;
            c.a = factor;
            wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].Color= Color.Lerp(c, wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].Color,.5f);
        }
        void meanAttach(int _x, int _y, int _z, Vector2Int v, float mean)
        {
            float h =  wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h;
            int sign = mean > h ? 1 : -1;
            wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h += .1f * sign * Mathf.Abs(mean - h);
        }
        void ShapeAttach(Event current)
        {
            if (terrainOps.Value == 1)
            {
                float sum = 0;
                ChunkInfo ci;
                Vector2Int chunk ;
                Vector3Int pos;
                foreach (var v in opState.selectedTerrainTilesDict.Keys)
                {
                    ci = v.ChunkInfoFromPos();
                    chunk = ci.chunk;
                    pos = ci.pos;
                    if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                        wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());
                    sum +=  wdc.WorldData.TerrainChunkHolder[chunk][pos.x , pos.y, pos.z ].h;
                }
                float mean = sum / (float)(opState.selectedTerrainTilesDict.Count);
                foreach (var v in opState.selectedTerrainTilesDict.Keys)
                {

                    ci = v.ChunkInfoFromPos();
                    chunk = ci.chunk;
                    pos = ci.pos;
                    if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);

                    meanAttach(pos.x , pos.y, pos.z, chunk, mean);
                }
            }
            else
            {
                foreach (var v in opState.selectedTerrainTilesDict.Keys)
                {
                    ChunkInfo ci = (v+new Vector3Int(1,0,1)).ChunkInfoFromPos();
                    Vector2Int chunk = ci.chunk;
                    Vector3Int pos = ci.pos;
                    if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                        wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());
                    if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);

                    terrainAttach(pos.x, pos.y, pos.z, chunk, opState.selectedTerrainTilesDict[v]);
                }
            }
            mouseDragStart = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
            opState.selectedTerrainTilesDict.Clear();
        }
        void ShapeErase(Event current)
        {
            foreach (var v in opState.selectedTerrainTilesDict.Keys)
            {
                ChunkInfo ci = v.ChunkInfoFromPos();
                Vector2Int chunk = ci.chunk;
                Vector3Int pos = ci.pos;
                if (!wdc.WorldData.TerrainChunkHolder.ContainsKey(chunk))
                    wdc.WorldData.TerrainChunkHolder.Add(chunk, new SerializableMultiArray<TerrainTileData>());
                if (!chunksToRepaint.Contains(chunk)) chunksToRepaint.Add(chunk);

                terrainErase(pos.x , pos.y, pos.z, chunk);
            }
            mouseDragStart = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
            opState.selectedTerrainTilesDict.Clear();
        }
        void meanErase(int _x, int _y, int _z, Vector2Int v, float mean)
        {
            float h = wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h;
            int sign = mean > h ? 1 : -1;
            wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h -= .1f * sign * Mathf.Abs(mean - h);
        }

        void terrainErase(int _x, int _y, int _z, Vector2Int v)
        {

            TerrainTileData ttd = wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z];
            switch (terrainOps.Value)
            {
                case 0:
                    wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].h -= .01f* brushStength.Value;
                    break;
                case 3:
                    Color c = Color.black;
                    c.a = 0;
                    wdc.WorldData.TerrainChunkHolder[v][_x, _y, _z].Color = c;
                    break;
                case 4:
                    if (wdc.WorldData.TerrainChunkHolder.Keys.Contains(v))
                        wdc.WorldData.DeleteTerrainKey(v);
                    break;
            }
        }
    }
}

