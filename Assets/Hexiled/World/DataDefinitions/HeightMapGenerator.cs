using UnityEngine;
using Hexiled.World.Data;

public static class HeightMapGenerator 
{
    //public int seed;
    //public bool randomizeSeed;

    //public int numOctaves = 7;
    //public float persistence = .5f;
    //public float lacunarity = 2;
    //public float initialScale = 2;

    //public bool useComputeShader = true;
    //public ComputeShader heightMapComputeShader;

    public static SerializableMultiArray<TerrainTileData> GenerateHeightMap(int mapSize,int seed, int numOctaves, float persistence,float lacunarity,float initialScale, Vector2 offset, ComputeShader heightMapComputeShader)
    {
        //if (useComputeShader)
        //{
            return GenerateHeightMapGPU(mapSize,seed,numOctaves,persistence,lacunarity,initialScale,offset, heightMapComputeShader);
        //}
        //return GenerateHeightMapCPU(mapSize);
    }

    public static SerializableMultiArray<TerrainTileData> GenerateHeightMapGPU(int mapSize, int seed, int numOctaves, float persistence, float lacunarity, float initialScale, Vector2 offset, ComputeShader heightMapComputeShader)
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
        SerializableMultiArray<TerrainTileData> _map = new SerializableMultiArray<TerrainTileData>();

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
        //Debug.Log("min:"+minValue);
        //Debug.Log(maxValue);
        for (int i = 0; i < map.Length; i++)
        {
            int y = i /(mapSize);
            int x = i % (mapSize);
            float t = map[i];

            t = Mathf.InverseLerp(numOctaves, -1, map[i]);
            _map[x, 0, y].h = t;
        }
       

        return _map;
    }

    //float[] GenerateHeightMapCPU(int mapSize)
    //{
    //    var map = new float[mapSize * mapSize];
    //    seed = (randomizeSeed) ? Random.Range(-10000, 10000) : seed;
    //    var prng = new System.Random(seed);

    //    Vector2[] offsets = new Vector2[numOctaves];
    //    for (int i = 0; i < numOctaves; i++)
    //    {
    //        offsets[i] = new Vector2(prng.Next(-1000, 1000), prng.Next(-1000, 1000));
    //    }

    //    float minValue = float.MaxValue;
    //    float maxValue = float.MinValue;

    //    for (int y = 0; y < mapSize; y++)
    //    {
    //        for (int x = 0; x < mapSize; x++)
    //        {
    //            float noiseValue = 0;
    //            float scale = initialScale;
    //            float weight = 1;
    //            for (int i = 0; i < numOctaves; i++)
    //            {
    //                Vector2 p = offsets[i] + new Vector2(x / (float)mapSize, y / (float)mapSize) * scale;
    //                noiseValue += Mathf.PerlinNoise(p.x, p.y) * weight;
    //                weight *= persistence;
    //                scale *= lacunarity;
    //            }
    //            map[y * mapSize + x] = noiseValue;
    //            minValue = Mathf.Min(noiseValue, minValue);
    //            maxValue = Mathf.Max(noiseValue, maxValue);
    //        }
    //    }

    //    // Normalize
    //    if (maxValue != minValue)
    //    {
    //        for (int i = 0; i < map.Length; i++)
    //        {
    //            map[i] = (map[i] - minValue) / (maxValue - minValue);
    //        }
    //    }

    //    return map;
    //}
}