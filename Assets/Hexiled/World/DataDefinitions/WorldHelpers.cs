using System;
using System.Collections.Generic;
using UnityEngine;
namespace Hexiled.World
{
    public static class WorldHelpers
    {
        public const int MapSize = 32;
        public static Vector2Int GetWorldPosFromChunkInfo(Vector2Int[] chunkInfo)
        {
            Vector2Int worldPos = new Vector2Int();
            worldPos.x = chunkInfo[0].x * MapSize + chunkInfo[1].x;
            worldPos.y = chunkInfo[0].y * MapSize + chunkInfo[1].y;
            return worldPos;
        }

        public static Vector2[] GetChunkInfoFromWorldPos(Vector2 worldPos)
        {
            Vector2[] chunkInfo = new Vector2[2];
            float chunkX = worldPos.x >= 0 ? worldPos.x / MapSize : (worldPos.x / MapSize) - 1;
            float chunkY = worldPos.y >= 0 ? worldPos.y / MapSize : (worldPos.y / MapSize) - 1;

            chunkInfo[0] = new Vector2((chunkX), (chunkY));
            float tileX = worldPos.x >= 0 ? worldPos.x % MapSize : MapSize + (worldPos.x % MapSize) - 1;
            float tileY = worldPos.y >= 0 ? worldPos.y % MapSize : MapSize + (worldPos.y % MapSize) - 1;

            chunkInfo[1] = new Vector2((tileX), (tileY));
            return chunkInfo;
        }
        public static Vector2Int[] GetIntChunkInfoFromWorldPos(Vector2 worldPos)
        {
            Vector2Int[] chunkInfo = new Vector2Int[2];
            int chunkX = worldPos.x >= 0 ? (int)worldPos.x / MapSize : ((int)worldPos.x / MapSize) - 1;
            int chunkY = worldPos.y >= 0 ? (int)worldPos.y / MapSize : ((int)worldPos.y / MapSize) - 1;

            chunkInfo[0] = new Vector2Int((chunkX), (chunkY));
            int tileX = worldPos.x >= 0 ? (int)worldPos.x % MapSize : MapSize + ((int)worldPos.x % MapSize) ;
            int tileY = worldPos.y >= 0 ? (int)worldPos.y % MapSize : MapSize + ((int)worldPos.y % MapSize) ;

            //int tileX = (int)worldPos.x % MapSize;
            //int tileY = (int)worldPos.y % MapSize;


            chunkInfo[1] = new Vector2Int((tileX), (tileY));
            return chunkInfo;
        }
        public static Vector2[] rotateUVS(Vector2[] uvs)
        {
            Vector2[] _uvs = new Vector2[4];
            _uvs[0] = uvs[1];
            _uvs[1] = uvs[2];
            _uvs[2] = uvs[3];
            _uvs[3] = uvs[0];
            return _uvs;
        }
        public static bool CompareUVS(Vector2[] uv1, Vector2[] uv2)
        {
            bool equal = true;

            for (int i = 0; i < uv1.Length; i++)
            {
                if (uv1[i] != uv2[i])
                    equal = false;

            }
            return equal;
        }

        public static bool CompareTopCenter(Vector2[] uv1, Vector2[] uv2)
        {
            return Math.Abs((UVCenter(uv1) - UVCenter(uv2)).sqrMagnitude) < .0001f;
        }

        public static Vector2 UVCenter(Vector2[] uv)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            for (int i = 0; i < uv.Length; i++)
            {
                minX = uv[i].x < minX ? uv[i].x : minX;
                minY = uv[i].y < minX ? uv[i].y : minY;
                maxX = uv[i].x > maxX ? uv[i].x : maxX;
                maxY = uv[i].y > maxY ? uv[i].y : maxY;
            }
            return new Vector2((maxX - minX) / 2f, maxY - minY / 2f) + new Vector2(minX, minY);
        }
        public static Vector2Int[] GetMaxMin(List<Vector3Int> vs)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;

            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (Vector3Int v in vs)
            {
                minX = v.x < minX ? v.x : minX;
                minY = v.z < minY ? v.z : minY;

                maxX = v.x > maxX ? v.x : maxX;
                maxY = v.z > maxY ? v.z : maxY;
            }
            Vector2Int[] ret = new Vector2Int[2];
            ret[0] = new Vector2Int(minX, minY);
            ret[1] = new Vector2Int(maxX, maxY);

            return ret;
        }
    }
}