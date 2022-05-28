using UnityEngine;
using Unity.Collections;
using Hexiled.ProceduralMeshes;
using Hexiled.ProceduralMeshes.Streams;

namespace Hexiled.ProceduralMeshes.Generators
{

	public interface IMeshGenerator
	{

		void Execute<S>(int i, S streams) where S : struct, IMeshStreams;

		Bounds Bounds { get; }

		int VertexCount { get; }

		int IndexCount { get; }

		int JobLength { get; }

		int Resolution { get; set; }

		NativeArray<TileDataStruct> Data { get; set; }
	}
}