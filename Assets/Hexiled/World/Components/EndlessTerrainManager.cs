using UnityEngine;
using Hexiled.World.SO;
using System.Collections.Generic;
using UnityEngine.Pool;
using Hexiled.ProceduralMeshes.Components;
namespace Hexiled.World.Components
{
    public class EndlessTerrainManager : MonoBehaviour
    {
        [SerializeField]
        GameObject meshObjectprefab;
        public float maxViewDst = 150;
        public Transform viewer;

        public static Vector2 viewerPosition;
            
        int chunksVisibleInViewDst;
        [SerializeField]
        public WorldData worldData;
        [SerializeField]
        GeneratorGraph generatorGraph;
        public ComputeShader heightMapComputeShader;

        public Dictionary<Vector2, GameObject> terrainChunkDictionary = new Dictionary<Vector2, GameObject>();

        [SerializeField] Vector2IntSO offsetSO;
        [SerializeField] Vector2IntSO currentChunk;
        [SerializeField]
        public float heighMultiplier;
        public Material terrainMaterial;

        ObjectPool<GameObject> _pool;

        void Start() { 
            _pool = new ObjectPool<GameObject>( ()=>Instantiate(meshObjectprefab,transform), (obj) => obj.SetActive(true), (obj) => obj.SetActive(false), actionOnDestroy: (obj) => Destroy(obj),  true, defaultCapacity: 10, 10);

            chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / 32);
            worldData.Terrains.Clear();
            UpdateVisibleChunks();
        }
        private void Update()
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
            UpdateVisibleChunks();
        }

        void UpdateVisibleChunks()
        {
            int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / 32);
            int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / 32);
            var keys = new Vector2[terrainChunkDictionary.Count];
            terrainChunkDictionary.Keys.CopyTo(keys, 0);
            foreach(Vector2 v in keys)
            {
                int LOD = CalculateLOD( v * 32);
                LOD = LOD == 0 ? 1 : LOD;
                GameObject go = terrainChunkDictionary[v];
                ProceduralMesh proceduralMesh = go.GetComponent<ProceduralMesh>();
                if (proceduralMesh.resolution < 32/LOD)
                {
                    proceduralMesh.PositionAndGenerate(v,32,LOD, maxViewDst);
                }
                if((viewerPosition-v*32).magnitude > maxViewDst)
                {
                    _pool.Release(go);
                    terrainChunkDictionary.Remove(v);
                }
            }
            
            for (int yOffset = -chunksVisibleInViewDst; yOffset < chunksVisibleInViewDst; yOffset++)
            {
                for (int xOffset = -chunksVisibleInViewDst; xOffset < chunksVisibleInViewDst; xOffset++)
                {
                    Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                    if (!terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        //Debug.Log((viewerPosition - viewedChunkCoord * 32).magnitude);
                        if ((viewerPosition - viewedChunkCoord * 32).magnitude < maxViewDst)
                        {
                            int LOD = CalculateLOD(viewedChunkCoord * 32);
                            GameObject go = _pool.Get();
                            ProceduralMesh pm = go.GetComponent<ProceduralMesh>();
                            pm.PositionAndGenerate(viewedChunkCoord, 32, LOD, maxViewDst);
                            terrainChunkDictionary.Add(viewedChunkCoord, go);
                        }
                    }
                    }
                }
            }
        int CalculateLOD(Vector2 pos)
        {
            Vector2 viewerPos = new Vector2(viewer.position.x, viewer.position.z) - new Vector2(32, 32  );
            int r =Mathf.FloorToInt(Mathf.Abs((pos - viewerPos).magnitude/48f));
            return r;
        }
    }
}
