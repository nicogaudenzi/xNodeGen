using UnityEngine;

public static class HeightMapGenerator 
{
    public static float[,] GenerateHeightMapGPU(int mapSize, int seed, int numOctaves, float persistence, float lacunarity, float initialScale, Vector2 offset, ComputeShader heightMapComputeShader)
    {
        var prng = new System.Random(seed);

        Vector2[] offsets = new Vector2[numOctaves];
        for (int i = 0; i < numOctaves; i++)
        {
            offsets[i] = new Vector2(prng.Next(-100000, 100000)+offset.x, prng.Next(-100000, 100000)+offset.y);
        }
        ComputeBuffer offsetsBuffer = new ComputeBuffer(offsets.Length, sizeof(float) * 2);
        offsetsBuffer.SetData(offsets);
        heightMapComputeShader.SetBuffer(0, "offsets", offsetsBuffer);

        int floatToIntMultiplier = 1000;
        float[] map = new float[(mapSize) * (mapSize)];
        float[,] _map = new float[mapSize, mapSize];

        ComputeBuffer mapBuffer = new ComputeBuffer(map.Length, sizeof(int));
        mapBuffer.SetData(map);
        heightMapComputeShader.SetBuffer(0, "heightMap", mapBuffer);

        //int[] minMaxHeight = { floatToIntMultiplier * numOctaves, 0 };
        int[] minMaxHeight = { numOctaves, -1 };
        ComputeBuffer minMaxBuffer = new ComputeBuffer(minMaxHeight.Length, sizeof(int));
        minMaxBuffer.SetData(minMaxHeight);
        heightMapComputeShader.SetBuffer(0, "minMax", minMaxBuffer);

        heightMapComputeShader.SetInt("mapSize", mapSize);
        heightMapComputeShader.SetInt("octaves", numOctaves);
        heightMapComputeShader.SetFloat("lacunarity", lacunarity);
        heightMapComputeShader.SetFloat("persistence", persistence);
        heightMapComputeShader.SetFloat("scaleFactor", initialScale);
        heightMapComputeShader.SetInt("floatToIntMultiplier", floatToIntMultiplier);

        heightMapComputeShader.Dispatch(0, map.Length, 1, 1);

        mapBuffer.GetData(map);
        minMaxBuffer.GetData(minMaxHeight);
        mapBuffer.Release();
        minMaxBuffer.Release();
        offsetsBuffer.Release();

        float minValue = (float)minMaxHeight[0] / (float)floatToIntMultiplier;
        float maxValue = (float)minMaxHeight[1] / (float)floatToIntMultiplier;
       
        for (int i = 0; i < map.Length; i++)
        {
            int y = i /(mapSize);
            int x = i % (mapSize);
            float t = map[i];

            t = Mathf.InverseLerp(numOctaves, -1, map[i]);
            _map[x,  y] = t;
        }
        return _map;
    }
}