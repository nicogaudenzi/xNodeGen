using UnityEngine;

public static class Vector3Extensions
{
    public static Vector2 Fomchunck3ToChunck2(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    public static Vector2Int Fomchunck3ToChunck2Int(this Vector3 v)
    {
        return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.z));
    }
    public static Vector3Int FloorToInt(this Vector3 v)
    {
        int _x = Mathf.FloorToInt(v.x);
        int _y = Mathf.FloorToInt(v.y);
        int _z = Mathf.FloorToInt(v.z);
        return new Vector3Int(_x, _y, _z);

    }
}