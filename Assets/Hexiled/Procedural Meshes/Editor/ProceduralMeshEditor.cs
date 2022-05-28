using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hexiled.ProceduralMeshes.Components;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
    private ProceduralMesh pm;

    private void OnEnable()
    {
        pm = (ProceduralMesh)target;
        //pm.chunkChanged.Event.AddListener(pm.OnChunckChanged);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate"))
        {
            pm.GenerateMeshFromEditor();
        }
    }
}
