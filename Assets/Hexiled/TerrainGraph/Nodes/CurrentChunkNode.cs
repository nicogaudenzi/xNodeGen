using Hexiled.World.SO;
using Hexiled.World.Data;
using UnityEngine;
using XNode;
[CreateNodeMenu("Scene/CurrentChunk")]
public class CurrentChunkNode:AbsGeneratorNode
{
    public CurrentChunk currentChunk;
    [Input] public TerrainChunkHolder terrainData;

    public override Generator GetGenerator()
    {
        return new ContainerGenerator(terrainData[currentChunk.Value]);
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
