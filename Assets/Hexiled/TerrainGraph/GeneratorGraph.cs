using UnityEngine;
using System.Collections.Generic;
using XNode;
using Hexiled.World.Data;
using Hexiled.World.Events;

[CreateAssetMenu(fileName ="Generator Graph",menuName ="Hexiled/TerrainGraph/GeneratorGraph")]
public class GeneratorGraph : NodeGraph
{ 
    public VoidEventSO graphChanged;
    

public Generator GetEndGenerator()
    {
        foreach (Node n in nodes)
        {
            if (n is EndNode)
            {
                return ((EndNode)n).GetFinalGenerator();
            }
        }

        return null;
    }
    public SerializableMultiArray<Color> GetColors()
    {
        foreach (Node n in nodes)
        {
            if (n is EndNode)
            {
                return ((EndNode) n).GetColors();
            }
        }

        return null;
    }
    public Generator GetUnprocessedNoise()
    {
        foreach (Node n in nodes)
        {
            if (n is EndNode)
            {
                return ((EndNode)n).GetUnprocessedNoise().GetGenerator();
            }
        }

        return null;
    }
}
