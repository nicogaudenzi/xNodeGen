using System;
using Hexiled.World.Data;
using UnityEngine;
public class TerrainPostProssesingGenerator : Generator
{

    public Generator terrainData;
    public Gradient gradient;
    public float heightMultiplier = 1;
    public AnimationCurve heightCurve;

    public TerrainPostProssesingGenerator(Gradient gradient, float heightMultiplier, AnimationCurve heightCurve, Generator terrainData)
    {
        this.gradient = gradient;
        this.heightMultiplier = heightMultiplier;
        this.heightCurve = heightCurve;
        this.terrainData = terrainData;
    }

    public override float GetValue(float x, float y, float z)
    {
        int i = (int)MathF.Floor(x);
        int j = (int)MathF.Floor(y);
        int k = (int)MathF.Floor(z);
        return heightCurve.Evaluate(terrainData.GetValue(i, 0, k)) * heightMultiplier;
    }
}
