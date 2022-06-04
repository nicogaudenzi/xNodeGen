using UnityEngine;
using Unity.Collections;
using Hexiled.World.Data;
using Hexiled.World.SO;
using Hexiled.World.Events;
using Hexiled.ProceduralMeshes.Jobs;

namespace Hexiled.ProceduralMeshes.Components { 
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]
	public class ProceduralMesh : MonoBehaviour {

		[SerializeField]
		public GeneratorGraph generatorGraph;
		[SerializeField] public Vector2IntSO offsetSO;
		[SerializeField] public Vector2IntSO currentChunk;
		[SerializeField] public Vector3EventSO chunkChanged;
		[SerializeField] public VoidEventSO graphChanged;
		[SerializeField] public Vector2IntEventSO askTerrainRepaint;

		[SerializeField]
		VoidEventSO TerrainSettingsChanged, FinishedEdition, worldDataChanged, rePaintTerrain, LODChanged;
		[SerializeField]
		BoolEventSO AutoUpdateChanged, terrainVisibilityChanged;
		
		public Vector2Int offset;
		public int LOD { get; set; }
		Mesh mesh,colliderMesh;

		public ComputeShader heightMapComputeShader;

		[SerializeField, Range(1, 32)]
		public int resolution = 32;

		[ReadOnly]
		NativeArray<TileDataStruct> heightData;

		[SerializeField]
		public float heightMultiplier;

		[SerializeField]
		BoolSO updateGraph;

		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				if (chunkChanged != null)
					chunkChanged.Event.AddListener(OnChunckChanged);
				if (graphChanged != null)
					graphChanged.Event.AddListener(GenerateMeshFromEditor);
				if (askTerrainRepaint != null)
					askTerrainRepaint.Event.AddListener(onAskTerrainRepaint);
				LODChanged.Event.AddListener(GenerateMeshFromEditor);
				FinishedEdition.Event.AddListener(GenerateMeshFromEditor);
				worldDataChanged.Event.AddListener(GenerateMeshFromEditor);
				rePaintTerrain.Event.AddListener(GenerateMeshFromEditor);

				TerrainSettingsChanged.Event.AddListener(GenerateMeshFromEditor);
			}
		}
		void Clean()
		{
			if (!Application.isPlaying)
			{
				chunkChanged.Event.RemoveListener(OnChunckChanged);
				graphChanged.Event.RemoveListener(GenerateMeshFromEditor);
				if (askTerrainRepaint != null)
					askTerrainRepaint.Event.RemoveListener(onAskTerrainRepaint);
			}
			
		}
		private void OnDisable() => Clean();
		private void OnDestroy() => Clean();

		public void OnChunckChanged(Vector3 v)
		{
			GenerateMeshFromEditor();

		}
		void onAskTerrainRepaint(Vector2Int v)
        {

            if (v == currentChunk.Value + offset)
            {
				GenerateMeshFromEditor();

			}


		}
		void Awake()
		{
			mesh = new Mesh
			{
				name = "Procedural Mesh"
			};
			GetComponent<MeshFilter>().mesh = mesh;

			colliderMesh = new Mesh
			{
				name = "Collider Mesh"
			};
			GetComponent<MeshCollider>().sharedMesh = colliderMesh;

		}
		private void OnValidate() {
	#if UNITY_EDITOR

			UnityEditor.EditorApplication.update += _OnValidate;
	#endif
			if (Application.isPlaying)
			{
				enabled = true;
			}
		}
			private void _OnValidate()
		{
			UnityEditor.EditorApplication.update -= _OnValidate;
			if (this == null) return;
			GenerateMeshFromEditor();
		}

		public void GenerateMeshFromEditor()
		{
			if (!updateGraph.Value) return;
			if(mesh==null)
			mesh = new Mesh
			{
				name = "Editor Procedural Mesh"
			};
			if(colliderMesh==null)
			colliderMesh = new Mesh
			{
				name = "Collider Mesh"
			};
			GetComponent<MeshCollider>().sharedMesh = colliderMesh;
			GetComponent<MeshFilter>().mesh = mesh;
			GenerateMesh((offset + currentChunk.Value) * 32);
		}
		public void GenerateMesh(Vector3 v)
		{
			GenerateMesh(new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.z)));
		}
			public void GenerateMesh(Vector2Int v) {

			//data = HeightMapGenerator.GenerateHeightMapGPU(resolution, 2, 3, 2, 1, 128, new Vector2(0, 0), heightMapComputeShader);
			offsetSO.Value = v;
			heightData = new NativeArray<TileDataStruct>(resolution * resolution, Allocator.TempJob);

			Generator fGenerator= generatorGraph.GetEndGenerator();
			SerializableMultiArray<Color> _colors = generatorGraph.GetColors();
			for (int i = 0; i < resolution; i++)
			{
				for (int j = 0; j < resolution; j++)
				{
					//heightData[i * resolution + j] = data[i,j]*heightMultiplier;
					heightData[j * resolution + i] =new TileDataStruct(fGenerator.GetValue(i, 0, j) * heightMultiplier,_colors[i,0,j]);
				}
			}
			Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
			Mesh.MeshData meshData = meshDataArray[0];

			DefinedMeshJob.ScheduleParallel(
				mesh,meshData,resolution, default, heightData
			).Complete();
			for (int i = 0; i < resolution; i++)
			{
				for (int j = 0; j < resolution; j++)
				{
					heightData[j * resolution + i] = new TileDataStruct(fGenerator.GetValue(i, 0, j) * heightMultiplier, _colors[i, 0, j]);
				}
			}
			Mesh.MeshDataArray meshDataArray1 = Mesh.AllocateWritableMeshData(1);
			Mesh.MeshData meshData1 = meshDataArray1[0];

			ColliderMeshJob.ScheduleParallel(
				colliderMesh, meshData1, resolution, default, heightData
			).Complete();
			Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

			Mesh.ApplyAndDisposeWritableMeshData(meshDataArray1, colliderMesh);
			heightData.Dispose();
		}
		public void PositionAndGenerate(Vector2 coord, int size, int LOD, float maxViewDist)
		{
			Vector2 _position = coord * size;
			Vector3 positionV3 = new Vector3(_position.x - .5f, 0, _position.y - .5f);
			transform.position = positionV3;
			this.LOD = LOD == 0 ? 1 : LOD;
			this.resolution = 32 / this.LOD;
			Vector2Int _offset = 32 * new Vector2Int(Mathf.FloorToInt(coord.x), Mathf.FloorToInt(coord.y));
			offset = _offset;
			offsetSO.Value = _offset;
			GenerateMesh(_offset);
		}
	}
}