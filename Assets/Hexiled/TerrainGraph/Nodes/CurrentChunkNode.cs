using Hexiled.World.SO;
using UnityEngine;
using XNode;
[CreateNodeMenu("Scene/CurrentChunk")]
public class CurrentChunkNode:AbsGeneratorNode
{
    public Vector2IntSO currentChunk;
    [Input] public TerrainChunkHolder terrainData;

    public override Generator GetGenerator()
    {
        var genNode1 = GetInputValue<TerrainChunkHolder>("terrainData");

        return new ContainerGenerator(genNode1[currentChunk.Value]);
    }

    public override string GetTitle()
    {
        return "CurrentChunk Selector";
    }


    //public override object GetValue(NodePort port)
    //{
    //    if (port.fieldName == "result")
    //    {
    //        Vector2Int v = currentChunk.Value;
    //         return terrainData[v];
    //    }

    //    else return null;
    //}



}
