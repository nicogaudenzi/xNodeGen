using UnityEngine;
using Unity.Mathematics;
namespace Hexiled.ProceduralMeshes.Streams 
{
	public struct Vertex
	{
		public float3 position, normal;
		public float4 tangent;
		public float2 texCoord0;
		public Color32 color;
	}
}
