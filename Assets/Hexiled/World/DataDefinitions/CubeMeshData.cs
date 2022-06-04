using UnityEngine;
using System;
using Hexiled.World.Data;

public static class CubeMeshData
{
    public static Vector3[] vertices = {
        new Vector3 (1, 1, 1),
        new Vector3 (-1, 1, 1),
        new Vector3 (-1, -1, 1),
        new Vector3 (1, -1, 1),
        new Vector3 (-1, 1, -1),
        new Vector3 (1, 1, -1),
        new Vector3 (1, -1, -1),
        new Vector3 (-1, -1, -1)
    };
    public static int[][] faceTriangles = {
        new int[] { 0, 1, 2, 3 },
        new int[] { 5, 0, 3, 6 },
        new int[] { 4, 5, 6, 7 },
        new int[] { 1, 4, 7, 2 },
        new int[] { 5, 4, 1, 0 },
        new int[] { 3, 2, 7, 6 }
    };

    public static Vector3[] faceVertices(int dir, float scale, Vector3 pos, float height, TileData neighbour)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            Vector3 currVertex = vertices[faceTriangles[dir][i]];
            float correction = neighbour == null || neighbour.tileType == TileType.Empty ? 0 : neighbour.height < height ? neighbour.height + 1 : height + 1;
            float correctedHeight = 0;

            if (neighbour != null)
            {
                if (neighbour.tileType == TileType.Mesh)
                {
                    correction = 0;
                }
                correctedHeight = currVertex.y < 0 ? correction : height;

            }
            else
            {
                correctedHeight = currVertex.y < 0 ? correction : height;
            }
            Vector3 v = vertices[faceTriangles[dir][i]];
            fv[i] = new Vector3(v.x * scale * .5f, v.y * .5f, v.z * scale * .5f) + pos + new Vector3(0, correctedHeight, 0);
        }
        return fv;
    }

    public static Vector3[] faceVertices(int dir, float scale, Vector3 pos, float height, float neighbour, bool correct)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            Vector3 currVertex = vertices[faceTriangles[dir][i]];
            float correction = neighbour < height ? neighbour + 1 : height + 1;
            float correctedHeight = currVertex.y < 0 ? correction : height;
            Vector3 v = vertices[faceTriangles[dir][i]];
            fv[i] = new Vector3(v.x * scale * .5f, v.y, v.z * scale * .5f) + pos + new Vector3(0, correctedHeight, 0);
        }
        return fv;
    }

    public static Vector3[] faceVertices(Direction dir, float scale, Vector3 pos, float height, TileData neighbour)
    {
        return faceVertices((int)dir, scale, pos, height, neighbour);
    }


}

public static class MeshPrefabs
{
    public static Vector3 rotate(Vector3 v)
    {
        return new Vector3(-v.z, v.y, v.x);
    }

    public static Vector3[] lowRampN = {
        new Vector3 (1, 1, 1),
        new Vector3 (-1, 1, 1),
        new Vector3 (-1, -1, 1),
        new Vector3 (1, -1, 1),
        new Vector3 (-1, -1, -1),
        new Vector3 (1, -1, -1),
        new Vector3 (1, -1, -1),
        new Vector3 (-1, -1, -1)
    };

    public static Vector3[] Corner = {
       new Vector3 (1, 1, 1),
        new Vector3 (-1, 1, 1),
        new Vector3 (-1, -1, 1),
        new Vector3 (1, -1, 1),
        new Vector3 (-1, 1, 1),
        new Vector3 (1, 1, -1),
        new Vector3 (1, -1, -1),
        new Vector3 (-1, -1, 1)
    };

    public static int[][] faceTriangles = {
        new int[] { 0, 1, 2, 3 },
        new int[] { 5, 0, 3, 6 },
        new int[] { 4, 5, 6, 7 },
        new int[] { 7, 2, 1, 4 },
        new int[] { 5, 4, 1, 0 },
        new int[] { 3, 2, 7, 6 }
    };

    public static Vector3[] RampVertex(Direction dir, float scale, Vector3 pos, float height, int meshIndex, int steep)
    {
        Vector3[] fv = new Vector3[4];
        float _steep = steep * .25f;
        for (int i = 0; i < fv.Length; i++)
        {

            Vector3 v = lowRampN[faceTriangles[(int)dir][i]];
            for (int j = 0; j < meshIndex; j++)
            {
                v = MeshPrefabs.rotate(v);
            }
            float correctedHeight = 0;
            correctedHeight = v.y > 0 ? height : _steep;
            fv[i] = new Vector3(v.x * scale * .5f, v.y * .5f, v.z * scale * .5f) + pos + new Vector3(0, correctedHeight, 0);
        }
        return fv;
    }
    public static Vector3[] CornerVertex(Direction dir, float scale, Vector3 pos, float height, TileData neighbour, int meshIndex, int steep)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {

            Vector3 v = Corner[faceTriangles[(int)dir][i]];
            for (int j = 0; j < meshIndex; j++)
            {
                v = MeshPrefabs.rotate(v);
            }

            fv[i] = new Vector3(v.x * scale * .5f, v.y * .5f, v.z * scale * .5f) + pos + new Vector3(0, height, 0);
        }
        return fv;
    }
}
