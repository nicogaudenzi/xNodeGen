using System;
using UnityEngine;
using System.Collections.Generic;
using Hexiled.World.Events;
using Hexiled.World.Data;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEditor;


namespace Hexiled.World.SO
{

    [Serializable]
    public class SerializableMeshInfo
    {
        [SerializeField]
        public Vector3[] vertices;
        [SerializeField]
        public int[] triangles;
        [SerializeField]
        public Vector2[] uv;
        [SerializeField]
        public Vector2[] uv2;
        [SerializeField]
        public Vector3[] normals;
        [SerializeField]
        public Color[] colors;

        [SerializeField]
        public int subMeshCount;
        [SerializeField]
        public UnityEngine.Rendering.SubMeshDescriptor[] subMeshDescriptors;

        public SerializableMeshInfo(Mesh m) // Constructor: takes a mesh and fills out SerializableMeshInfo data structure which basically mirrors Mesh object's parts.
        {
            vertices = new Vector3[m.vertexCount]; // initialize vertices array.
            Array.Copy(m.vertices, vertices, m.vertexCount);
            //for (int i = 0; i < m.vertexCount; i++) // Serialization: Vector3's values are stored sequentially.
            //{
            //    vertices[i ] = m.vertices[i];
            //}
            triangles = new int[m.triangles.Length]; // initialize triangles array
            Array.Copy(m.triangles, triangles, m.triangles.Length);

            //for (int i = 0; i < m.triangles.Length; i++) // Mesh's triangles is an array that stores the indices, sequentially, of the vertices that form one face
            //{
            //    triangles[i] = m.triangles[i];
            //}
            uv = new Vector2[m.uv.Length]; // initialize uvs array
            Array.Copy(m.uv, uv, m.uv.Length);

            //for (int i = 0; i < m.uv.Length; i++) // uv's Vector2 values are serialized similarly to vertices' Vector3
            //{
            //    uv[i] = m.uv[i];
            //}
            //uv2 = new Vector2[m.uv2.Length]; // uv2
            //for (int i = 0; i < m.uv2.Length; i++)
            //{
            //    uv[i] = m.uv2[i];
            //}
            normals = new Vector3[m.normals.Length];
            Array.Copy(m.normals, normals, m.normals.Length);

            //for (int i = 0; i <m.normals.Length; i++)
            //{
            //    normals[i] = m.normals[i];
            //}
            colors = new Color[m.colors.Length];
            Array.Copy(m.colors, colors, m.colors.Length);

            //for (int i = 0; i < m.colors.Length; i++)
            //{
            //    colors[i] = m.colors[i];
            //}
            subMeshCount = m.subMeshCount;
            subMeshDescriptors = new UnityEngine.Rendering.SubMeshDescriptor[m.subMeshCount];

            for (int i = 0; i < m.subMeshCount; i++)
            {
                subMeshDescriptors[i] = m.GetSubMesh(i);
            }
        }

        // GetMesh gets a Mesh object from currently set data in this SerializableMeshInfo object.
        // Sequential values are deserialized to Mesh original data types like Vector3 for vertices.
        public Mesh GetMesh()
        {
            Mesh m = new Mesh();
            List<Vector3> verticesList = new List<Vector3>();
            for (int i = 0; i < vertices.Length; i++)
            {
                verticesList.Add(
                        vertices[i]);
            }
            m.SetVertices(verticesList);
            m.triangles = triangles;
            List<Vector2> uvList = new List<Vector2>();
            for (int i = 0; i < uv.Length / 2; i++)
            {
                uvList.Add(uv[i]);
            }
            m.SetUVs(0, uvList);
            List<Vector2> uv2List = new List<Vector2>();
            for (int i = 0; i < uv2.Length / 2; i++)
            {
                uv2List.Add(uv2[i]);
            }
            m.SetUVs(1, uv2List);

            m.colors = colors;

            return m;
        }
    }

    [Serializable]
    public class BakedMeshHolder : SerializableDictionaryBase<Vector2, SerializableMeshInfo> { }
    [Serializable]
    public class CollidersHolder : SerializableDictionaryBase<Vector2, SerializableMeshInfo> { }
    [Serializable]
    public class BakedTerrainHolder : SerializableDictionaryBase<Vector2, SerializableMeshInfo> { }

    [CreateAssetMenu(fileName = "Baked World Data", menuName = "Hexiled/World/Baked Meshes Holder")]
    public class BakedWorldData : ScriptableObject
    {
        [SerializeField]
        BakedMeshHolder bakedMeshes;
        [SerializeField]
        BakedMeshHolder bakedTerrain;
        [SerializeField]
        CollidersHolder bakedColliders;
        [SerializeField]
        CollidersHolder bakedTerrainColliders;
        //[SerializeField]
        VoidEventSO bakedFinished;
        public BakedMeshHolder BakedMeshes { get => bakedMeshes; set => bakedMeshes = value; }
        public CollidersHolder BakedColliders { get => bakedColliders; set => bakedColliders = value; }
        public BakedMeshHolder BakedTerrain { get => bakedTerrain; set => bakedTerrain = value; }
        public CollidersHolder BakedTerrainColliders { get => bakedTerrainColliders; set => bakedTerrainColliders = value; }

        //private void OnEnable()
        //{
        //    if (bakedFinished == null)
        //        bakedFinished = (VoidEventSO)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "bakedFinished" + ".asset");
        //}
        public void BakeAll(WorldData worlddata)
        {
            Baked b = Baker.Bake(worlddata);
            BakedMeshes = b.BakedMeshes;
            BakedTerrain = b.BakedTerrain;
            BakedColliders = b.BakedColliders;
            BakedTerrainColliders = b.BakedTerrainColliders;
            bakedFinished.Event.Invoke();
        }

    }

    public struct Baked
    {
        [SerializeField]
        BakedMeshHolder bakedMeshes;
        [SerializeField]
        BakedMeshHolder bakedTerrain;
        [SerializeField]
        CollidersHolder bakedColliders;
        [SerializeField]
        CollidersHolder bakedTerrainColliders;
        [SerializeField]
        VoidEventSO bakedFinished;
        public BakedMeshHolder BakedMeshes { get => bakedMeshes; set => bakedMeshes = value; }
        public CollidersHolder BakedColliders { get => bakedColliders; set => bakedColliders = value; }
        public BakedMeshHolder BakedTerrain { get => bakedTerrain; set => bakedTerrain = value; }
        public CollidersHolder BakedTerrainColliders { get => bakedTerrainColliders; set => bakedTerrainColliders = value; }
    }
    public static class Baker
    {
        static List<Vector3> vertices, terrainVertex, colliderVertex, colliderTerrainVertex;
        static List<Vector2> uv;
        static List<int> triangles, terrainTriangles, colliderTriangles, colliderTerrainTriangles;
        static List<Color> colors, terrainColors;
        static List<int>[] subTriangles;
        static SerializableMultiArray<TileData> map;
        static WorldData data;

        public static Baked Bake(WorldData worldData)
        {
            data = worldData;
            Baked b = new Baked();
            b.BakedMeshes = new BakedMeshHolder();
            b.BakedTerrain = new BakedMeshHolder();
            b.BakedColliders = new CollidersHolder();
            b.BakedTerrainColliders = new CollidersHolder();

            foreach (var chunck in data.WorldChuncks.Keys)
            {
                Mesh m = new Mesh();
                StartTileOven(data, chunck);
                if (m != null)
                {
                    m.Clear();
                    m.vertices = vertices.ToArray();
                }
                if (data.WorldTilemaps.TileMaps.Length == 0)
                {
                    m.triangles = triangles.ToArray();
                }
                else
                {

                    m.subMeshCount = subTriangles.Length;
                    for (int i = 0; i < subTriangles.Length; i++)
                    {
                        m.SetTriangles(subTriangles[i].ToArray(), i);
                    }
                }
                m.uv = uv.ToArray();
                m.colors = colors.ToArray();
                m.RecalculateNormals();
                m.RecalculateBounds();
                m.name = chunck.ToString();
                SerializableMeshInfo meshInfo = new SerializableMeshInfo(m);
                b.BakedMeshes.Add(chunck, meshInfo);

                m = new Mesh();
                m.vertices = terrainVertex.ToArray();
                m.triangles = terrainTriangles.ToArray();
                m.colors = terrainColors.ToArray();
                m.name = chunck.ToString() + "Terrain";
                SerializableMeshInfo terrainMeshInfo = new SerializableMeshInfo(m);
                b.BakedTerrain.Add(chunck, terrainMeshInfo);

                m = new Mesh();
                m.vertices = colliderVertex.ToArray();
                m.triangles = colliderTriangles.ToArray();
                m.name = chunck.ToString() + "ColliderMesh";
                b.BakedColliders.Add(chunck, new SerializableMeshInfo(m));

                m = new Mesh();
                m.vertices = colliderTerrainVertex.ToArray();
                m.triangles = colliderTerrainTriangles.ToArray();
                m.name = chunck.ToString() + "ColliderTerrainMesh";
                b.BakedTerrainColliders.Add(chunck, new SerializableMeshInfo(m));
            }
            foreach (var chunck in data.TerrainChunkHolder.Keys)
            {
                if (data.WorldChuncks.ContainsKey(chunck))
                    continue;
                Mesh m = new Mesh();
                StartTileOvenTerrainOnly(data, chunck);
                if (m != null)
                {
                    m.Clear();
                    m.vertices = vertices.ToArray();
                }
                if (data.WorldTilemaps.TileMaps.Length == 0)
                {
                    m.triangles = triangles.ToArray();
                }
                else
                {
                    m.subMeshCount = subTriangles.Length;
                    for (int i = 0; i < subTriangles.Length; i++)
                    {
                        m.SetTriangles(subTriangles[i].ToArray(), i);
                    }
                }

                m = new Mesh();
                m.vertices = terrainVertex.ToArray();
                m.triangles = terrainTriangles.ToArray();
                m.colors = terrainColors.ToArray();
                m.name = chunck.ToString() + "Terrain";
                SerializableMeshInfo terrainMeshInfo = new SerializableMeshInfo(m);
                b.BakedTerrain.Add(chunck, terrainMeshInfo);

                m = new Mesh();
                m.vertices = colliderTerrainVertex.ToArray();
                m.triangles = colliderTerrainTriangles.ToArray();
                m.name = chunck.ToString() + "ColliderTerrainMesh";
                b.BakedTerrainColliders.Add(chunck, new SerializableMeshInfo(m));
            }
            return b;
        }

        public static BakedMeshHolder BakeTiles(WorldData worlddata)
        {
            data = worlddata;
            BakedMeshHolder baked = new BakedMeshHolder();
            foreach (var chunck in data.WorldChuncks.Keys)
            {
                Mesh m = new Mesh();
                StartTileOven(data, chunck);
                if (m != null)
                {
                    m.Clear();
                    m.vertices = vertices.ToArray();
                }
                if (data.WorldTilemaps.TileMaps.Length == 0)
                {
                    m.triangles = triangles.ToArray();
                }
                else
                {

                    m.subMeshCount = subTriangles.Length;
                    for (int i = 0; i < subTriangles.Length; i++)
                    {
                        m.SetTriangles(subTriangles[i].ToArray(), i);
                    }
                }
                m.uv = uv.ToArray();
                m.colors = colors.ToArray();
                m.RecalculateNormals();
                m.RecalculateBounds();
                m.name = chunck.ToString();
                SerializableMeshInfo meshInfo = new SerializableMeshInfo(m);

                baked.Add(chunck, meshInfo);


            }
            return baked;

        }
        static void StartTileOven(WorldData data, Vector2 chunkCoords)
        {

            vertices = new List<Vector3>();
            terrainVertex = new List<Vector3>();
            colliderVertex = new List<Vector3>();
            colliderTerrainVertex = new List<Vector3>();

            triangles = new List<int>();
            terrainTriangles = new List<int>();
            colliderTriangles = new List<int>();
            colliderTerrainTriangles = new List<int>();
            uv = new List<Vector2>();
            colors = new List<Color>();
            terrainColors = new List<Color>();
            subTriangles = new List<int>[data.WorldTilemaps.TileMaps.Length];
            for (int i = 0; i < subTriangles.Length; i++)
            {
                subTriangles[i] = new List<int>();
            }

            if (!data.WorldChuncks.ContainsKey(chunkCoords)) return;
            map = data.WorldChuncks[chunkCoords];
            for (int y = 0; y < data.WorldChuncks[chunkCoords].MapHeight; y++)
            {
                for (int z = 0; z < WorldHelpers.MapSize; z++)
                {
                    for (int x = 0; x < WorldHelpers.MapSize; x++)
                    {
                        Vector3 adjPosition = new Vector3((float)x, (float)y, (float)z);
                        DrawCube(1, adjPosition, x, y, z, chunkCoords);
                    }
                }
            }
        }
        static void DrawCube(float cubeScale, Vector3 cubePos, int x, int y, int z, Vector2 selectedChunk)
        {
            //int _scale = LOD.Value == 0 ? 1 : 2 * LOD.Value;
            int _scale = 1;
            if (map[x, y, z].tileType != TileType.Empty)
            {
                for (int i = 0; i < 5; i++)
                {
                    TileData neighbor = map.GetNeighbor(x, y, z, (Direction)i);
                    float heightDifference = neighbor == null ? map[x, y, z].height : (map[x, y, z].height - neighbor.height);
                    DrawFace((Direction)i, _scale, cubePos, map[x, y, z], neighbor, selectedChunk);
                }
            }
            else
            {
                float heightCorrection;
                if (data.TerrainChunkHolder.ContainsKey(selectedChunk))
                {
                    SerializableMultiArray<TerrainTileData> tch = data.TerrainChunkHolder[selectedChunk];
                    heightCorrection = tch[x, 0, z].h;
                }
                else
                {
                    heightCorrection = 0;
                }
                for (int i = 0; i < 5; i++)
                {
                    DrawTerrainFace((Direction)i, _scale, cubePos, data.Terrains[selectedChunk][z, 0, x] + heightCorrection, 0, 0);
                }
            }
        }

        static void DrawFace(Direction dir, float faceScale, Vector3 facePos, TileData t, TileData neighbour, Vector2 selectedChunk)
        {
            if (Math.Abs(t.height - 1) < .00001f && dir != Direction.Up) return;

            float heightDifference = neighbour == null ? t.height : (t.height - neighbour.height);

            Vector3[] myFaceVertex;

            if (t.tileType == TileType.Mesh && t.meshType != MeshType.Prefab)
            {
                if (t.meshType == MeshType.Ramp)
                    myFaceVertex = MeshPrefabs.RampVertex(dir, faceScale, facePos, t.height, (int)t.meshRot, (int)t.meshSteep);
                else
                {
                    myFaceVertex = MeshPrefabs.CornerVertex(dir, faceScale, facePos, t.height, neighbour, (int)t.meshRot, (int)t.meshSteep);
                }

            }
            else
            {
                myFaceVertex = CubeMeshData.faceVertices(dir, faceScale, facePos, t.height, neighbour);
            }
            for (int i = 0; i < myFaceVertex.Length; i++)
            {
                myFaceVertex[i] += Vector3.up * data.TileHeightHolder[selectedChunk];
            }

            vertices.AddRange(myFaceVertex);
            if (dir == Direction.Up && t.walkable)
            {
                colliderVertex.AddRange(myFaceVertex);
            }
            Vector2[] uvs;
            uvs = dir == Direction.Up ? t.UVs : t.bottomUVs;
            for (int i = 0; i < 4; i++)
            {
                float correctUV = 0;
                if ((i == 2 || i == 3) && dir != Direction.Up)
                {
                    correctUV = Math.Abs(heightDifference) < .00001f ? 0 : Math.Abs((1 - heightDifference) * (uvs[0].y - uvs[2].y));
                    if (neighbour == null || neighbour.tileType == TileType.Empty || neighbour.tileType == TileType.Mesh)
                    {
                        correctUV = Math.Abs((t.height) * (uvs[0].y - uvs[2].y));
                    }
                }


                uv.Add(new Vector2(uvs[i].x, uvs[i].y + correctUV));

                colors.Add(t.color);

            }

            int vCount = vertices.Count;
            int matIndex = dir == Direction.Up ? t.topIndex : t.botIndex;
            if (!(data.WorldTilemaps.TileMaps.Length == 0) && matIndex != -1)
            {
                if (subTriangles[matIndex] == null) subTriangles[matIndex] = new List<int>();
                subTriangles[matIndex].Add(vCount - 4);
                subTriangles[matIndex].Add(vCount - 4 + 1);
                subTriangles[matIndex].Add(vCount - 4 + 2);
                subTriangles[matIndex].Add(vCount - 4);
                subTriangles[matIndex].Add(vCount - 4 + 2);
                subTriangles[matIndex].Add(vCount - 4 + 3);

            }
            triangles.Add(vCount - 4);
            triangles.Add(vCount - 4 + 1);
            triangles.Add(vCount - 4 + 2);
            triangles.Add(vCount - 4);
            triangles.Add(vCount - 4 + 2);
            triangles.Add(vCount - 4 + 3);
            if (dir == Direction.Up && t.walkable)
            {
                colliderVertex.AddRange(myFaceVertex);
                int collidervCount = colliderVertex.Count;

                colliderTriangles.Add(collidervCount - 4);
                colliderTriangles.Add(collidervCount - 4 + 1);
                colliderTriangles.Add(collidervCount - 4 + 2);
                colliderTriangles.Add(collidervCount - 4);
                colliderTriangles.Add(collidervCount - 4 + 2);
                colliderTriangles.Add(collidervCount - 4 + 3);
            }
        }
        static void DrawTerrainFace(Direction dir, float faceScale, Vector3 facePos, float t, float neighbour, float correctHeight)
        {
            if (Math.Abs(t - 1) < .00001f && dir != Direction.Up) return;
            float h = data.TerrainSettings.heightCurve.Evaluate(t + correctHeight / 16) * data.TerrainSettings.heightMultiplier;
            Vector3[] myFaceVertex;

            //if (useCap && dir == Direction.Up)
            //{
            //    myFaceVertex = new Vector3[cap.vertices.Length];
            //    for (int i = 0; i < cap.vertices.Length; i++)
            //    {

            //        myFaceVertex[i] = cap.vertices[i] + facePos + Vector3.up * h;
            //    }
            //}
            //else
            //{
            myFaceVertex = CubeMeshData.faceVertices((int)dir, faceScale, facePos, h, neighbour * data.TerrainSettings.heightMultiplier, true);

            //}

            terrainVertex.AddRange(myFaceVertex);



            int vCount = terrainVertex.Count;
            //if (useCap && dir == Direction.Up)
            //{
            //    for (int i = 0; i < cap.vertices.Length; i++)
            //    {
            //        colors.Add(wdc.WorldData.TerrainSettings.gradient.Evaluate(t + correctHeight / (2 * wdc.WorldData.TerrainSettings.heightMultiplier)));
            //    }
            //    int vertStart = vCount - cap.vertices.Length;
            //    for (int i = 0; i < cap.triangles.Length; i++)
            //    {
            //        triangles.Add(vertStart + cap.triangles[i]);
            //    }
            //}
            //else
            //{
            for (int i = 0; i < 4; i++)
            {
                terrainColors.Add(data.TerrainSettings.gradient.Evaluate(t + correctHeight / (2 * data.TerrainSettings.heightMultiplier)));
            }
            terrainTriangles.Add(vCount - 4);
            terrainTriangles.Add(vCount - 4 + 1);
            terrainTriangles.Add(vCount - 4 + 2);
            terrainTriangles.Add(vCount - 4);
            terrainTriangles.Add(vCount - 4 + 2);
            terrainTriangles.Add(vCount - 4 + 3);
            if (dir == Direction.Up)
            {
                colliderTerrainVertex.AddRange(myFaceVertex);
                int collidervCount = colliderTerrainVertex.Count;

                colliderTerrainTriangles.Add(collidervCount - 4);
                colliderTerrainTriangles.Add(collidervCount - 4 + 1);
                colliderTerrainTriangles.Add(collidervCount - 4 + 2);
                colliderTerrainTriangles.Add(collidervCount - 4);
                colliderTerrainTriangles.Add(collidervCount - 4 + 2);
                colliderTerrainTriangles.Add(collidervCount - 4 + 3);
            }
            //}
        }
        static void StartTileOvenTerrainOnly(WorldData data, Vector2 chunkCoords)
        {

            terrainVertex = new List<Vector3>();
            colliderTerrainVertex = new List<Vector3>();
            terrainTriangles = new List<int>();
            colliderTerrainTriangles = new List<int>();
            terrainColors = new List<Color>();

            if (!data.WorldChuncks.ContainsKey(chunkCoords)) return;
            map = data.WorldChuncks[chunkCoords];
            for (int y = 0; y < data.WorldChuncks[chunkCoords].MapHeight; y++)
            {
                for (int z = 0; z < WorldHelpers.MapSize; z++)
                {
                    for (int x = 0; x < WorldHelpers.MapSize; x++)
                    {
                        Vector3 adjPosition = new Vector3((float)x, (float)y, (float)z);
                        DrawCubeTerrainOnly(1, adjPosition, x, y, z, chunkCoords);
                    }
                }
            }
        }
        static void DrawCubeTerrainOnly(float cubeScale, Vector3 cubePos, int x, int y, int z, Vector2 selectedChunk)
        {
            //int _scale = LOD.Value == 0 ? 1 : 2 * LOD.Value;
            int _scale = 1;

            float heightCorrection;
            if (data.TerrainChunkHolder.ContainsKey(selectedChunk))
            {
                SerializableMultiArray<TerrainTileData> tch = data.TerrainChunkHolder[selectedChunk];
                heightCorrection = tch[x, 0, z].h;
            }
            else
            {
                heightCorrection = 0;
            }
            for (int i = 0; i < 5; i++)
            {
                DrawTerrainFaceTerrainOnly((Direction)i, _scale, cubePos, data.Terrains[selectedChunk][z, 0, x] + heightCorrection, 0, 0);
            }
        }
        static void DrawTerrainFaceTerrainOnly(Direction dir, float faceScale, Vector3 facePos, float t, float neighbour, float correctHeight)
        {
            if (Math.Abs(t - 1) < .00001f && dir != Direction.Up) return;
            float h = data.TerrainSettings.heightCurve.Evaluate(t + correctHeight / 16) * data.TerrainSettings.heightMultiplier;
            Vector3[] myFaceVertex;

            myFaceVertex = CubeMeshData.faceVertices((int)dir, faceScale, facePos, h, neighbour * data.TerrainSettings.heightMultiplier, true);



            terrainVertex.AddRange(myFaceVertex);



            int vCount = terrainVertex.Count;

            for (int i = 0; i < 4; i++)
            {
                terrainColors.Add(data.TerrainSettings.gradient.Evaluate(t + correctHeight / (2 * data.TerrainSettings.heightMultiplier)));
            }
            terrainTriangles.Add(vCount - 4);
            terrainTriangles.Add(vCount - 4 + 1);
            terrainTriangles.Add(vCount - 4 + 2);
            terrainTriangles.Add(vCount - 4);
            terrainTriangles.Add(vCount - 4 + 2);
            terrainTriangles.Add(vCount - 4 + 3);
            if (dir == Direction.Up)
            {
                colliderTerrainVertex.AddRange(myFaceVertex);
                int collidervCount = colliderTerrainVertex.Count;

                colliderTerrainTriangles.Add(collidervCount - 4);
                colliderTerrainTriangles.Add(collidervCount - 4 + 1);
                colliderTerrainTriangles.Add(collidervCount - 4 + 2);
                colliderTerrainTriangles.Add(collidervCount - 4);
                colliderTerrainTriangles.Add(collidervCount - 4 + 2);
                colliderTerrainTriangles.Add(collidervCount - 4 + 3);
            }
            //}
        }
    }
}
