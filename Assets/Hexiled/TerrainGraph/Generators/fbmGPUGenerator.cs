using System;
using UnityEngine;
using Hexiled.World.Data;
public class fbmGPUGenerator : Generator
{
    SerializableMultiArray<TerrainTileData> data;
    public int mapSize, seed, numOctaves, initialScale;
    public float persistence, lacunarity;
    public Vector2 offset;


    public ComputeShader computeShader;

    public fbmGPUGenerator(int mapSize, int seed, int numOctaves, float persistence, float lacunarity, int initialScale, Vector2 offset, ComputeShader computeShader)
    {
        this.mapSize = mapSize;
        this.seed = seed;
        this.numOctaves = numOctaves;
        this.persistence = persistence;
        this.lacunarity = lacunarity;
        this.initialScale = initialScale;
        this.offset = offset;
        this.computeShader = computeShader;
    }
    public void Generate(int mapSize, int seed, int numOctaves, float persistence, float lacunarity, float initialScale, Vector2 offset, ComputeShader heightMapComputeShader)
    {
        data = HeightMapGenerator.GenerateHeightMapGPU(mapSize, seed, numOctaves, persistence, lacunarity, initialScale, offset, computeShader);
    }
    public override float GetValue(float x, float y, float z)
    {
        int i = (int)MathF.Floor(x);
        int j = (int)MathF.Floor(y);
        int k = (int)MathF.Floor(z);
        //if (i == 15) Debug.Log(data[i, 0, k].h);
        return data[i, 0, k].h;
    }
}
