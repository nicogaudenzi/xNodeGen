﻿using Hexiled.World.Editor;
using UnityEditor;
using UnityEngine;
using Hexiled.World.SO;
using Hexiled.World.Events;
public class WorldSettingEditorWindow : EditorWindow
{
    [SerializeField]
    WorldDataContainer wdc;
    [SerializeField]
    BoolSO settingEventHandler;
    [SerializeField]
    IntSO editorBar;
    WorldGeneralEditor generalEditor;
    LayerOpsEditor layerOpsEditor;
    [SerializeField]
    OperationsState opState;
    [SerializeField]
    VoidEventSO layerVisibilityChanged, LayersChanged, worldDataChanged;
    [SerializeField]
    Vector3EventSO ChunckChanged;
    [SerializeField]
    Vector2IntSO selectedChunk;
    public IntSO EditorBar { get => editorBar; set => editorBar = value; }
    [MenuItem("Tools/Hexiled/Yeti3D/World Settings")]
    static void Init()
    {
        var window = GetWindow<WorldSettingEditorWindow>(false, "World Settings");
        window.ShowUtility();
    }

    private void OnEnable()
    {
        LayersChanged.Event.AddListener(Repaint);
        ChunckChanged.Event.AddListener(UpdateLayerVisibility);
        worldDataChanged.Event.AddListener(UpdateLayerVisibility);
    }

    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUILayout.Space(10);
            GUILayout.Label("World Settings should not be edited during runtime");
            return;
        }

        GUILayout.Space(25);
        EditorGUILayout.LabelField("Current WorldData: " + wdc.WorldData.name);

        GUILayout.Space(25);
        editorBar.Value = GUILayout.Toolbar(editorBar.Value,
                                    new string[] { "General", "Layers" });
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        switch (editorBar.Value)
        {
            case 0:
                if (generalEditor == null)
                {
                    RecycleGeneralEditor();
                }
                generalEditor.OnInspectorGUI();
                break;
            case 1:

                LayerVisibility();

                if (layerOpsEditor == null)
                {
                    RecycleLayerOpsEditor();
                }
                GUILayout.Space(5);
                if (opState.EditingLayer != 0)
                {
                    opState.useBottomLayerAsMask = GUILayout.Toggle(opState.useBottomLayerAsMask, "Use Bottom Layer As Mask");
                    GUILayout.Space(5);

                }
                GUILayout.Label("Layer Operations");
                GUILayout.Space(5);
                layerOpsEditor.OnInspectorGUI();
                break;

        }
    }
    void UpdateLayerVisibility(Vector3 v)
    {
        UpdateLayerVisibility();
    }
        void UpdateLayerVisibility()
    {
        opState.layerVisibility.Clear();
        Vector2Int chunck = selectedChunk.Value;
        if (wdc.WorldData.WorldChuncks.ContainsKey(chunck))
        {
            for (int i = 0; i < wdc.WorldData.WorldChuncks[chunck].MapHeight; i++)
            {
                opState.layerVisibility.Add(true);
            }
        }
        else
        {
            opState.layerVisibility.Add(true);
        }
        layerVisibilityChanged.Event.Invoke();
        Repaint();
    }
    void LayerVisibility()
    {
        GUILayout.Space(5);

        GUILayout.Label("Layer Visibility");
        GUILayout.Space(5);
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < opState.layerVisibility.Count; i++)
        {
            opState.layerVisibility[i] = GUILayout.Toggle(opState.layerVisibility[i], i.ToString());
        }
        if (EditorGUI.EndChangeCheck())
        {
            layerVisibilityChanged.Event.Invoke();
        }

    }
    void RecycleGeneralEditor()
    {
        if (generalEditor != null) DestroyImmediate(generalEditor);
        generalEditor = Editor.CreateEditor(settingEventHandler, typeof(WorldGeneralEditor)) as WorldGeneralEditor;

    }
    void RecycleLayerOpsEditor()
    {
        if (layerOpsEditor != null) DestroyImmediate(layerOpsEditor);
        layerOpsEditor = Editor.CreateEditor(settingEventHandler, typeof(LayerOpsEditor)) as LayerOpsEditor;
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
    private void OnDestroy()
    {
        Clean();
    }
    private void OnDisable()
    {
        Clean();
    }
    void Clean()
    {
        if (generalEditor != null) DestroyImmediate(generalEditor);
        if (layerOpsEditor != null) DestroyImmediate(layerOpsEditor);
        LayersChanged.Event.RemoveListener(Repaint);
        ChunckChanged.Event.RemoveListener(UpdateLayerVisibility);
        worldDataChanged.Event.RemoveListener(UpdateLayerVisibility);
    }
}
