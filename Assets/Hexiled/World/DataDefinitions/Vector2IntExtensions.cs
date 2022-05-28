using UnityEngine;

public static class Vector2IntExtensions
{
    public static Vector3Int ToVector3Int_addY(this Vector2Int v)
    {
        return new Vector3Int(v.x,0, v.y);
    }
}
