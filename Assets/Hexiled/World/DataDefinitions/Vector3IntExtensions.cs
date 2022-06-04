using System;
using UnityEngine;
public struct ChunkInfo
{
    public Vector2Int chunk;
    public Vector3Int pos;
}
public static class Vector3IntExtensions
{
    public static ChunkInfo ChunkInfoFromPos(this Vector3Int v)
    {
        int chunckX = v.x / 32;
        if (v.x < 0 && v.x % 32==0) chunckX = chunckX+1;
        chunckX =(v.x < 0 ) ? chunckX - 1 : chunckX;
        //if(v.x != 32 * chunckX)
        //{
        //    if(v.x<0)
        //       chunckX = chunckX + 1;
        //}
        int chunckY = v.z / 32;
        if (v.z < 0 && v.z % 32==0) chunckY = chunckY+1;
        chunckY = (v.z < 0 ) ? chunckY - 1 : chunckY;

        int posX = v.x % 32;
        if (v.x < 0 && posX == 0) posX = -32;
        posX = v.x < 0 ? 32 + posX : posX;
        int posZ = v.z % 32;
        if (v.z < 0 && posZ == 0) posZ = -32;
        posZ = v.z < 0 ? 32 + posZ : posZ;
       
        Vector2Int _chunk = new Vector2Int(chunckX,chunckY);
        Vector3Int _pos = new Vector3Int(posX,0,posZ);
        ChunkInfo ci = new ChunkInfo();
        ci.chunk = _chunk;
        ci.pos = _pos;
        return ci;
    }
}
