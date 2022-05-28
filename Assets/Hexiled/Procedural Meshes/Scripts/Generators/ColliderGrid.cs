using UnityEngine;
using Unity.Collections;
using static Unity.Mathematics.math;
using Hexiled.ProceduralMeshes.Streams;

namespace Hexiled.ProceduralMeshes.Generators
{


	public struct ColliderGrid : IMeshGenerator
	{
		const int numberofFaces = 1;

		public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(16f, 0f, 16f));

		public int VertexCount => numberofFaces * 4 * Resolution * Resolution;

		public int IndexCount => numberofFaces * 6 * Resolution * Resolution;

		public int JobLength => Resolution;

		public int Resolution { get; set; }

		public NativeArray<TileDataStruct> Data { get; set; }

		public void Execute<S>(int z, S streams) where S : struct, IMeshStreams
		{

			int vi = numberofFaces * 4 * Resolution * z, ti = numberofFaces * 2 * Resolution * z;
			for (int x = 0; x < Resolution; x++, vi += numberofFaces * 4, ti += numberofFaces * 2)
			{
				int dataIndex = z * Resolution + x;

				var xCoordinates = 32 * (float2(x, x + 1f) / Resolution);
				var zCoordinates = 32 * (float2(z, z + 1f) / Resolution);

				var vertex = new Vertex();
				//Top Face
				vertex.normal.y = 1f;
				//vertex.tangent.xw = float2(1f, -1f);

				vertex.position.y = Data[dataIndex].h;
				vertex.position.x = xCoordinates.x;
				vertex.position.z = zCoordinates.x;

				streams.SetVertex(vi + 0, vertex);

				vertex.position.x = xCoordinates.y;
				vertex.texCoord0 = float2(1f, 0f);

				streams.SetVertex(vi + 1, vertex);

				vertex.position.x = xCoordinates.x;
				vertex.position.z = zCoordinates.y;
				vertex.texCoord0 = float2(0f, 1f);

				streams.SetVertex(vi + 2, vertex);

				vertex.position.x = xCoordinates.y;
				vertex.texCoord0 = 1f;

				streams.SetVertex(vi + 3, vertex);

				streams.SetTriangle(ti + 0, vi + int3(0, 2, 1));
				streams.SetTriangle(ti + 1, vi + int3(1, 2, 3));
			}
		}


	}
}