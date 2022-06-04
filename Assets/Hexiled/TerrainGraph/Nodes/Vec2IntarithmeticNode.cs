using UnityEngine;
using System.Collections;
using XNode;
[CreateNodeMenu("Input/Vec2IntArithmetic")]
public class Vec2IntarithmeticNode : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public Vector2Int A;
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public Vector2Int B;
    [Output] public Vector2Int result;
    public override object GetValue(NodePort port)
    {
        return A+B;
    }
}
