using System;
using UnityEngine;
using XNode;

[CreateNodeMenu("Input/Vector2")]
public class Vector2Holder:XNode.Node
{
    [Output] public Vector2Int output;
    public Vector2IntSO vector;
        public override object GetValue(NodePort port)
    {
        return vector.Value;
    }
}
