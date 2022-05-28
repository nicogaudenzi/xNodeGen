using UnityEngine;
using System.Collections.Generic;
using XNode;
using Hexiled.World.Data;
using Hexiled.World.Events;

[CreateAssetMenu]
public class GeneratorGraph : NodeGraph
{ 
    public VoidEventSO graphChanged;
    //private List<GraphEventListener> listeners =
    //    new List<GraphEventListener>();

    //public void Raise()
    //{
    //    for (int i = listeners.Count - 1; i >= 0; i--)
    //        listeners[i].OnEventRaised();
    //}
    //public bool Contains(GraphEventListener gel)
    //{
    //    return listeners.Contains(gel) ? true : false;
    //}
    //public void RegisterListener(GraphEventListener listener)
    //{ listeners.Add(listener); }

    //public void UnregisterListener(GraphEventListener listener)
    //{ listeners.Remove(listener); }


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
   }
