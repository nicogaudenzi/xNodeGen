using UnityEngine;
namespace Hexiled.ProceduralMeshes
{
	[System.Serializable]
	public struct ColliderDataStruct
	{
		public float h;
		public Vector2Int v;
		public ColliderDataStruct(float _h, Vector2Int _v)
		{
			h = _h;
			v = _v;
		}
	}
}