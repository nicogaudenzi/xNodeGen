﻿using UnityEngine;
using UnityEditor;
using Hexiled.World.SO;
using Hexiled.World.Events;
using System;

public class WorldGeneralEditor : Editor
{
    [SerializeField]
    WorldDataContainer wdc;
    [SerializeField]
    IntSO LOD;
    [SerializeField]
    VoidEventSO worldDataChanged, finishedEdition, askToBake,
        askToInstanciate, askToBakeCollider,
        askToInstanciateCollider, combineMeshes,LODChanged, askToBakeMeshes, askMeshRepaint;
    [SerializeField]
    Vector2IntSO selectedChunk;
    [SerializeField]
    BakedWorldData bwd;
    [SerializeField]
    OperationsState opState;
    Vector2Int currentCopy;
    string newDefineCompileConstant = "NAV_MESH_COMPONENT_PRESENT";

        bool setNav = false;
    BuildTarget buildtarget;
    BuildTargetGroup group;
    private void OnEnable()
    {
       
        buildtarget = EditorUserBuildSettings.activeBuildTarget;
        group = BuildPipeline.GetBuildTargetGroup(buildtarget);
       
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            setNav = defines.Contains(newDefineCompileConstant);
    }
    public override void OnInspectorGUI()
    {

             GUILayout.Space(5);

        
        GUILayout.Space(5);
        if (GUILayout.Button("Add TileMap", GUILayout.Width(300)))
        {
            wdc.WorldData.AddTileKey(selectedChunk.Value);
            EditorUtility.SetDirty(wdc.WorldData);
            AssetDatabase.SaveAssets();

        }
        if (GUILayout.Button("Add Terrain", GUILayout.Width(300)))
        {
            wdc.WorldData.AddTerrainKey(selectedChunk.Value);
            EditorUtility.SetDirty(wdc.WorldData);
            AssetDatabase.SaveAssets();
        }
        GUILayout.Space(5);
        GUILayout.Space(5);
        if (GUILayout.Button("Delete TileMap", GUILayout.Width(300)))
        {
            EditorUtility.SetDirty(wdc.WorldData);
            AssetDatabase.SaveAssets();
            wdc.WorldData.DeleteTileKey(selectedChunk.Value);
        }
        if (GUILayout.Button("Delete Terrain", GUILayout.Width(300)))
        {
            EditorUtility.SetDirty(wdc.WorldData);
            AssetDatabase.SaveAssets();
            wdc.WorldData.DeleteTerrainKey(selectedChunk.Value);
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Copy Terrain", GUILayout.Width(300)))
        {
            currentCopy = selectedChunk.Value;
        }
        if (GUILayout.Button("Paste Terrain", GUILayout.Width(300)))
        {
            wdc.WorldData.AddTerrainKey(selectedChunk.Value);
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    wdc.WorldData.TerrainChunkHolder[selectedChunk.Value][i, 0, j].Color = wdc.WorldData.TerrainChunkHolder[currentCopy][i, 0, j].Color;
                    wdc.WorldData.TerrainChunkHolder[selectedChunk.Value][i, 0, j].h = wdc.WorldData.TerrainChunkHolder[currentCopy][i, 0, j].h;
                }
            }
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Bake", GUILayout.Width(300)))
        {

            if (EditorUtility.DisplayDialog("Continue baking?",
                "This may overwrite the previously saved prefab. No undo possible", "Continue", "Cancel"))
            {
                askToBake.Event.Invoke();
            }
        }
        if (GUILayout.Button("Bake Combined Meshes", GUILayout.Width(300)))
        {
            askToBakeMeshes.Event.Invoke();
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Clean Meshes", GUILayout.Width(300)))
        {
            Undo.RegisterCompleteObjectUndo(wdc.WorldData, "Clean Meshes");
            wdc.WorldData.MesheHolder.Clear();
            askMeshRepaint.Event.Invoke();
        }
        GUILayout.Space(5);


        EditorGUI.BeginChangeCheck();
        setNav = GUILayout.Toggle(setNav,"NavMesh Components Present");
        if (EditorGUI.EndChangeCheck()) {
            
            BuildTargetGroup[] targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));
          
                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                if (!defines.Contains(newDefineCompileConstant))
                {
                    if (defines.Length > 0)         //if the list is empty, we don't need to append a semicolon first
                        defines += ";";
                    if (setNav)
                    {
                        defines += newDefineCompileConstant;
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defines);
                    }
                }
                else
                {
                if (!setNav)
                {
                    int index = defines.IndexOf(newDefineCompileConstant);
                    string cleanPath = (index < 0)
                        ? defines
                        : defines.Remove(index, newDefineCompileConstant.Length);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, cleanPath);
                }
            }
        }
    }

}