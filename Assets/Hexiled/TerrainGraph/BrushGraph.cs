using UnityEngine;
using System.Collections.Generic;
using XNode;
using Hexiled.World.Data;
using Hexiled.World.Events;

[CreateAssetMenu(fileName = "Brush Graph", menuName = "Hexiled/TerrainGraph/Brush Graph")]
public class BrushGraph : NodeGraph
{
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
                return ((EndNode)n).GetColors();
            }
        }

        return null;
    }
}
