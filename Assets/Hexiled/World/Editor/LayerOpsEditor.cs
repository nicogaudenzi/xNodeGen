using Hexiled.World.SO;
using Hexiled.World.Data;
using Hexiled.World.Events;
using UnityEditor;
using UnityEngine;

namespace Hexiled.World.Editor
{
    public class LayerOpsEditor : UnityEditor.Editor
    {
        [SerializeField]
        WorldDataContainer wdc;
        [SerializeField]
        Vector2IntSO selectedChunk;
        [SerializeField]
        IntSO editingLayer;
        [SerializeField]
        VoidEventSO layersChanged, askRepaint, deletedTopLayer, setLayerVisibility, undoAsked, askMeshRepaint, repainTerrain;

        [SerializeField]
        OperationsState opState;

        private void OnEnable()
        {
            //if (wdc == null)
            //    wdc = (WorldDataContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "World Data Container" + ".asset");
            //if (selectedChunk == null)
            //    selectedChunk = (Vector3SO)EditorGUIUtility.LoadRequired(InternalPaths.vector3SOs + "SelectedChunk" + ".asset");
            //if (editingLayer == null)
            //    editingLayer = (IntSO)EditorGUIUtility.LoadRequired(InternalPaths.intSOs + "CurrentLayer" + ".asset");
            //if (layersChanged == null)
            //    layersChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "LayersChanged" + ".asset");
            //if (askRepaint == null)
            //    askRepaint = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "askedForTileRepaint" + ".asset");
            //if (deletedTopLayer == null)
            //    deletedTopLayer = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "DeletedTopLayer" + ".asset");
            //if (setLayerVisibility == null)
            //    setLayerVisibility = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "SetLayerVisibility" + ".asset");
            //if (undoAsked == null)
            //    undoAsked = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "LayerUndo" + ".asset");
            //if (askMeshRepaint == null)
            //    askMeshRepaint = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "askMeshesRender" + ".asset");
            //if (repainTerrain == null)
            //    repainTerrain = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "RepaintTerrain" + ".asset");
            //if (opState == null)
            //    opState = (OperationsState)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "Operations State" + ".asset");

            Vector2Int chunck = selectedChunk.Value;

            SetLayerVisibility(chunck);
            undoAsked.Event.AddListener(SetLayerVisibilityUndo);
        }
        void Clean()
        {
            undoAsked.Event.RemoveListener(SetLayerVisibilityUndo);
        }
        void OnDestroy() => Clean();
        void OnDisable() => Clean();
        public override void OnInspectorGUI()
        {

            if (GUILayout.Button("Add Layer", GUILayout.Width(300)))
            {
                AddLayer();
            }
            if (GUILayout.Button("Fill Layer", GUILayout.Width(300)))
            {
                FillLayer();
            }
            if (GUILayout.Button("Clean Layer", GUILayout.Width(300)))
            {
                CleanLayer();
            }
            if (GUILayout.Button("Delete Top Layer", GUILayout.Width(300)))
                if (EditorUtility.DisplayDialog("Are you sure?",
                    "You are about to delete a Layer. Are you sure?",
                    "Yes",
                    "No"))
                {
                    DeleteTopLayer();
                }
            if (GUILayout.Button("Extrude Layer Below", GUILayout.Width(300)))
            {
                ExtrudeLayerBelow();
            }
            if (GUILayout.Button("Copy Layer Below", GUILayout.Width(300)))
            {
                CopyLayerBelow();
            }

            if (GUILayout.Button("Reset Chunk Data", GUILayout.Width(300)))
            {
                if (EditorUtility.DisplayDialog("Are you sure?",
                    "You are about to delete Everything from the World Data. Are you sure?",
                    "Yes",
                    "No"))
                {
                    Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Reset Meshes");

                    wdc.WorldData.ResetData();
                    askRepaint.Event.Invoke();
                }
            }

        }

        void AddLayer()
        {
            UnityEngine.Object[] objects = new UnityEngine.Object[3] { wdc.WorldData, opState, editingLayer };

            Undo.RegisterCompleteObjectUndo(objects, "Add Layer");
            EditorUtility.SetDirty(wdc.WorldData);
            Vector2Int chunck = selectedChunk.Value;
            wdc.WorldData.WorldChuncks[chunck].AddEmptyLayer();

            editingLayer.Value++;
            opState.layerVisibility.Add(true);
            layersChanged.Event.Invoke();
        }
        void SetLayerVisibilityUndo()
        {
            Vector2Int chunck = selectedChunk.Value;
            opState.layerVisibility = new System.Collections.Generic.List<bool>();
            for (int i = 0; i < wdc.WorldData.WorldChuncks[chunck].MapHeight; i++)
            {
                opState.layerVisibility.Add(true);
            }
            //setLayerVisibility.Raise();
            //layersChanged.Raise();
        }
        void SetLayerVisibility(Vector2Int chunck)
        {
            opState.layerVisibility = new System.Collections.Generic.List<bool>();
            for (int i = 0; i < wdc.WorldData.WorldChuncks[chunck].MapHeight; i++)
            {
                opState.layerVisibility.Add(true);
            }
            setLayerVisibility.Event.Invoke();
        }
        void FillLayer()
        {
            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Fill Layer");

            Vector2Int chunck = selectedChunk.Value;

            for (int z = 0; z < WorldHelpers.MapSize; z++)
            {
                for (int x = 0; x < WorldHelpers.MapSize; x++)
                {
                    int _x = x;
                    int _y = editingLayer.Value;
                    int _z = z;
                    TileType tt = wdc.WorldData.WorldChuncks[chunck][_x, _y, _z].tileType;
                    wdc.WorldData.WorldChuncks[chunck][_x, _y, _z].tileType = tt != TileType.Empty ? tt : TileType.Ground;
                }
            }
            askRepaint.Event.Invoke();
            repainTerrain.Event.Invoke();
        }

        void ExtrudeLayerBelow()
        {
            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Extrude Layer Below");

            Vector2Int chunck = selectedChunk.Value;

            for (int z = 0; z < SerializableMultiArray<TileData>.MapSize; z++)
            {
                for (int x = 0; x < SerializableMultiArray<TileData>.MapSize; x++)
                {
                    int _x = x;
                    int _y = editingLayer.Value;
                    int _z = z;
                    if (_y > 0)
                    {
                        TileData n = wdc.WorldData.WorldChuncks[chunck][_x, _y - 1, _z];
                        if (n.height != 0) continue;
                        if (n != null && n.tileType != TileType.Empty && wdc.WorldData.WorldChuncks[chunck].MapHeight >= 0)
                        {

                            wdc.WorldData.WorldChuncks[chunck][_x, _y, _z] = new TileData(n);
                        }
                    }
                }
            }
            askRepaint.Event.Invoke();
        }
        void CopyLayerBelow()
        {
            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Copy Layer Below");

            Vector2Int chunck = selectedChunk.Value;

            for (int z = 0; z < SerializableMultiArray<TileData>.MapSize; z++)
            {
                for (int x = 0; x < SerializableMultiArray<TileData>.MapSize; x++)
                {
                    int _x = x;
                    int _y = editingLayer.Value;
                    int _z = z;
                    if (_y > 0)
                    {
                        TileData n = wdc.WorldData.WorldChuncks[chunck][_x, _y - 1, _z];
                        if (n != null && n.tileType != TileType.Empty && wdc.WorldData.WorldChuncks[chunck].MapHeight >= 0)
                        {

                            wdc.WorldData.WorldChuncks[chunck][_x, _y, _z] = new TileData(n);
                        }
                    }
                }
            }
            askRepaint.Event.Invoke();
        }

        void CleanLayer()
        {
            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Clean Layer");

            for (int z = 0; z < WorldHelpers.MapSize; z++)
            {
                for (int x = 0; x < WorldHelpers.MapSize; x++)
                {
                    int _x = x;
                    int _y = editingLayer.Value;
                    int _z = z;
                    Vector3Int _v = new Vector3Int(_x, _y, z);
                    if (wdc.WorldData.MesheHolder.ContainsKey(_v))
                    {
                        wdc.WorldData.MesheHolder.Remove(_v);
                    }
                    Vector2Int v = selectedChunk.Value;
                    wdc.WorldData.WorldChuncks[v][_x, _y, _z] = new TileData();
                }
            }
            askRepaint.Event.Invoke();
            askMeshRepaint.Event.Invoke();
            repainTerrain.Event.Invoke();
        }

        void DeleteTopLayer()
        {
            Vector2Int chunck = selectedChunk.Value;
            SerializableMultiArray<TileData> cd = wdc.WorldData.WorldChuncks[chunck];
            if (editingLayer.Value == cd.MapHeight) editingLayer.Value--;
            UnityEngine.Object[] objects = new UnityEngine.Object[3] { wdc.WorldData, opState, editingLayer };

            Undo.RegisterCompleteObjectUndo(objects, "Delete Top Layer");

            if (cd.MapHeight > 1)
            {
                cd.DeleteTopLayer();
                //editingLayer.Value = 0;
                SetLayerVisibility(chunck);
                deletedTopLayer.Event.Invoke();
                Repaint();
            }
        }
    }
}
