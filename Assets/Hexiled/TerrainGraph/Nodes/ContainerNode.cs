using XNode;
using UnityEngine;
using Hexiled.World.SO;
[CreateNodeMenu("Containers")]
public class ContainerNode : Node

{
    public WorldData worldData;
    [Output] public WorldData data;
    [Output] public TerrainChunkHolder terrainData;
    public override object GetValue(NodePort port)
    {
        // Check which output is being requested. 
        // In this node, there aren't any other outputs than "result".
        if (port.fieldName == "data")
        {
            // Return input value + 1
            return worldData;
        }
        else if (port.fieldName == "terrain") return worldData.TerrainChunkHolder;
        // Hopefully this won't ever happen, but we need to return something
        // in the odd case that the port isn't "result"
        else return null;
    }
    //public override Generator GetGenerator()
    //{
    //    ContainerGenerator generator = new ContainerGenerator(terrainData[Vector2.zero]);
    //    generator.data = terrainData[Vector2.zero];
    //    return generator;
    //}
    //public override string GetTitle()
    //{
    //    return "Container";
    //}
}
