using XNodeEditor;
using UnityEditor;
using Hexiled.World.Events;
using UnityEngine;
[CustomNodeEditor(typeof(GraphupdateNode))]
public class graphUpdateNodeEditor : NodeEditor
{
    GraphupdateNode gup;
    Editor graphupdateEditor;
   
    void OnEnable()
    {
        
    }
    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        if (gup == null) gup = target as GraphupdateNode;
        if (graphupdateEditor == null) graphupdateEditor = Editor.CreateEditor(gup.updateGraph);
        graphupdateEditor.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                GeneratorGraph generatorGraph = gup.graph as GeneratorGraph;
                if (generatorGraph.graphChanged != null)
                {
                bool preV = gup.updateGraph.Value;
                    gup.updateGraph.Value = true;
                    generatorGraph.graphChanged.Event?.Invoke();
                    gup.updateGraph.Value = preV;

                }
            }
    }
}
