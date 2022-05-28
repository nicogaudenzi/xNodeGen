using UnityEngine;
using Unity.Collections;
using static Unity.Mathematics.math;
using Hexiled.ProceduralMeshes.Streams;
namespace Hexiled.ProceduralMeshes.Generators {

	public struct SquareGrid : IMeshGenerator
	{
		const int numberofFaces = 5;

		public Bounds Bounds => new Bounds(new Vector3(16,16,16), new Vector3(32f, 32f, 32f));

		public int VertexCount => numberofFaces*4 * Resolution * Resolution;

		public int IndexCount => numberofFaces*6 * Resolution * Resolution;

		public int JobLength => Resolution;

		public int Resolution { get; set; }

		public NativeArray<TileDataStruct> Data { get; set; }

		public void Execute<S>(int z, S streams) where S : struct, IMeshStreams
		{

			int vi = numberofFaces * 4 * Resolution * z, ti = numberofFaces * 2 * Resolution * z;
			for (int x = 0; x < Resolution; x++, vi += numberofFaces * 4, ti += numberofFaces * 2)
			{
				int dataIndex = z * Resolution + x;

				var xCoordinates =32* (float2(x, x + 1f)/Resolution  );
				var zCoordinates =32* (float2(z, z + 1f)/Resolution  );

				var vertex = new Vertex();
				vertex.color = Data[dataIndex].color;
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

				//West Face


				vertex.normal.y = 0f;
                vertex.normal.x = -1f;

                vertex.position.y = 0;
                vertex.position.x = xCoordinates.x;
                vertex.position.z = zCoordinates.x;
                streams.SetVertex(vi + 4, vertex);

                vertex.position.y = Data[dataIndex].h;
                vertex.position.x = xCoordinates.x;
                vertex.texCoord0 = float2(1f, 0f);
                streams.SetVertex(vi + 5, vertex);


                vertex.position.y = 0;
                vertex.position.x = xCoordinates.x;
                vertex.position.z = zCoordinates.y;
                vertex.texCoord0 = float2(0f, 1f);
                streams.SetVertex(vi + 6, vertex);

                vertex.position.y = Data[dataIndex].h;
                vertex.position.x = xCoordinates.x;
                vertex.texCoord0 = 1f;
                streams.SetVertex(vi + 7, vertex);
				streams.SetTriangle(ti + 4, vi + int3(4, 6, 5));
				streams.SetTriangle(ti + 5, vi + int3(5, 6, 7));
				//East Face

				vertex.normal.x = 1f;

				vertex.position.y = 0;
				vertex.position.x = xCoordinates.y;
				vertex.position.z = zCoordinates.x;
				streams.SetVertex(vi + 8, vertex);

				vertex.position.y = Data[dataIndex].h;
				vertex.texCoord0 = float2(1f, 0f);
				streams.SetVertex(vi + 9, vertex);


				vertex.position.y = 0;
				vertex.position.x = xCoordinates.y;
				vertex.position.z = zCoordinates.y;
				vertex.texCoord0 = float2(0f, 1f);
				streams.SetVertex(vi + 10, vertex);

				vertex.position.y = Data[dataIndex].h;
				vertex.texCoord0 = 1f;
				streams.SetVertex(vi + 11, vertex);
				streams.SetTriangle(ti + 2, vi + int3(9, 10, 8));
				streams.SetTriangle(ti + 3, vi + int3(11, 10, 9));

                //Nord Face

                vertex.normal.x = 0f;
                vertex.normal.z = 1f;

                vertex.position.y = 0;
                vertex.position.x = xCoordinates.x;
                vertex.position.z = zCoordinates.y;
                streams.SetVertex(vi + 12, vertex);


                vertex.position.y = Data[dataIndex].h;
                vertex.texCoord0 = float2(1f, 0f);
                streams.SetVertex(vi + 13, vertex);

                vertex.position.y = 0;

                vertex.position.x = xCoordinates.y;
                vertex.position.z = zCoordinates.y;
                vertex.texCoord0 = float2(0f, 1f);
                streams.SetVertex(vi + 14, vertex);

                vertex.position.y = Data[dataIndex].h;

                vertex.texCoord0 = 1f;
                streams.SetVertex(vi + 15, vertex);

                streams.SetTriangle(ti + 6, vi + int3(12, 14, 13));
                streams.SetTriangle(ti + 7, vi + int3(13, 14, 15));

                //South Face

                vertex.normal.x = 0f;
                vertex.normal.z = 1f;

                vertex.position.y = 0;
				vertex.position.x = xCoordinates.x;
				vertex.position.z = zCoordinates.x;
				streams.SetVertex(vi + 16, vertex);


				vertex.position.y = Data[dataIndex].h;
				vertex.texCoord0 = float2(1f, 0f);
				streams.SetVertex(vi + 17, vertex);

				vertex.position.y = 0;

				vertex.position.x = xCoordinates.y;
				vertex.position.z = zCoordinates.x;
				vertex.texCoord0 = float2(0f, 1f);
				streams.SetVertex(vi + 18, vertex);

				vertex.position.y = Data[dataIndex].h;

				vertex.texCoord0 = 1f;
				streams.SetVertex(vi + 19, vertex);

                streams.SetTriangle(ti + 8, vi + int3(17, 18, 16));
                streams.SetTriangle(ti + 9, vi + int3(19, 18, 17));

			}
		}

        
    }
}