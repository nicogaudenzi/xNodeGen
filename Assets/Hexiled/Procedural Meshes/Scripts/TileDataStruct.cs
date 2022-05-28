using UnityEngine;
namespace Hexiled.ProceduralMeshes{
	[System.Serializable]
	public struct TileDataStruct
	{
		public float h;
		public Color color;
		public TileDataStruct(float _h,Color _color)
		{
			h = _h;
			color = _color;
		}
}
}