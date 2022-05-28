using System;
using Hexiled.World.Data;

public class ContainerGenerator: Generator
{
    public SerializableMultiArray<TerrainTileData> data;

    public ContainerGenerator(SerializableMultiArray<TerrainTileData> data)
    {
        this.data = data;
    }
    public override float GetValue(float x, float y, float z)
    {
        int i = (int)MathF.Floor(x);
        int j = (int)MathF.Floor(y);
        int k = (int)MathF.Floor(z);

        return data[i, j, k].h;
    }
}
