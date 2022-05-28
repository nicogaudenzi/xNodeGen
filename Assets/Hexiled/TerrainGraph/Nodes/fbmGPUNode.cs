using System;
using XNode;
using UnityEngine;
using Hexiled.World.Data;
[CreateNodeMenu("Noise/fbmGRP")]
public class fbmGPUNode:AbsGeneratorNode
{
   
    public int mapSize,seed,numOctaves, initialScale;
    public float persistence, lacunarity;
    [Input] public Vector2Int Offset;

    public ComputeShader computeShader;
    fbmGPUGenerator fbmGPUgenerator;
    public override Generator GetGenerator()
    {
        
        //if(fbmGPUgenerator==null)
        fbmGPUgenerator = new fbmGPUGenerator(mapSize, seed, numOctaves, persistence, lacunarity, initialScale, GetInputValue<Vector2Int>("Offset"), computeShader);

        fbmGPUgenerator.Generate(mapSize, seed, numOctaves, persistence, lacunarity, initialScale, GetInputValue<Vector2Int>("Offset"), computeShader);
        return fbmGPUgenerator;
    }

    public override string GetTitle()
    {
        return "fbmGPU";
    }

    //public override object GetValue(NodePort port)
    //{
    //    data = HeightMapGenerator.GenerateHeightMapGPU(mapSize, seed, numOctaves, persistence, lacunarity, initialScale, offset, computeShader);

    //    if (port.fieldName == "noise")
    //    {
    //        return data;
    //    }

    //    else return null;
    //}
}
