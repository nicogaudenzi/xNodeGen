using Hexiled.World.Data;
using Hexiled.World.SO;
using UnityEngine;
using XNode;
[CreateNodeMenu("Containers")]
public class ContainerNode : AbsGeneratorNode

{
    [SerializeField]
    public WorldData worldData;
    [SerializeField]
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public Vector2Int Offset;
    [Output] public SerializableMultiArray<Color> Colors;
    SerializableMultiArray<Color> colors;

    public override Generator GetGenerator()
    {
        Vector2Int offset = GetInputValue<Vector2Int>("Offset")/32;
        if (!worldData.TerrainChunkHolder.ContainsKey(offset)) return new Constant(0);
        ContainerGenerator generator = new ContainerGenerator(worldData.TerrainChunkHolder[offset]);
        //    generator.data = terrainData[Vector2.zero];
        return generator;
    }
    public override string GetTitle()
    {
        return "Container";
    }

    public void FillColors()
    {
        Vector2Int offset = GetInputValue<Vector2Int>("Offset") / 32;

        colors = new SerializableMultiArray<Color>();
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                Color c = Color.black;
                c.a = 0;
                if (!worldData.TerrainChunkHolder.ContainsKey(offset)) c.a = 0;
                else c = worldData.TerrainChunkHolder[offset][i, 0, j].Color;
                colors[i, 0, j] = c;
            }
        }
    }
    public override object GetValue(NodePort port)
    {

        // Check which output is being requested. 
        // In this node, there aren't any other outputs than "result".
        if (port.fieldName == "Colors")
        {
            FillColors();
            // Return input value + 1
            return colors;
        }
        else if (port.fieldName == "Output")
        {
            return this;
        }
        // Hopefully this won't ever happen, but we need to return something
        // in the odd case that the port isn't "result"
        else return null;
    }
}
