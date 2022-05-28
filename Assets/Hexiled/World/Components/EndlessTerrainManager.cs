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
                GameObject go = terrainChunkDictionary[v];
                if ((go.transform.position - viewer.transform.position).magnitude > maxViewDst)
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
                    
                        if ((viewerPosition - viewedChunkCoord * 32).magnitude < maxViewDst)
                        {
                            int LOD = CalculateLOD(viewerPosition, viewedChunkCoord * 32);
                            GameObject go = _pool.Get();
                            ProceduralMesh pm = go.GetComponent<ProceduralMesh>();
                            pm.PositionAndGenerate(viewedChunkCoord, 32, LOD, maxViewDst);
                            terrainChunkDictionary.Add(viewedChunkCoord, go);
                        }

                    }
                }
            }
        int CalculateLOD(Vector3 pos, Vector3 viewerPos)
        {
            int r = Mathf.RoundToInt((pos - viewerPos).magnitude) / 32;
            r = r > 5 ? 5 : r - 1 < 0 ? 0 : r - 1;
            return r;
        }
    }
}
