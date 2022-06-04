using UnityEngine;
using Hexiled.World.SO;
//using UnityEditor;

namespace Hexiled.World.Components
{
    public class GridDrawer : MonoBehaviour
    {
        [SerializeField]
        //[HideInInspector]
        Vector2IntSO currentChunck;

        [SerializeField]
        //[HideInInspector]
        IntSO currentLayer;
        [SerializeField]
        //[HideInInspector]
        //Hexiled.World.SO.WorldDataContainer wdc;
        BoolSO showGrid;

        Vector3 offset = Vector3.one * (-.5f);

        private void OnDrawGizmosSelected()
        {
            if (!showGrid.Value) return;

            for (int i = 0; i < WorldHelpers.MapSize + 1; i++)
            {

                Vector3 startPos = new Vector3(i, this.currentLayer.Value, 0) +  new Vector3(currentChunck.Value.x, 0, currentChunck.Value.y) * WorldHelpers.MapSize;
                Vector3 endPos = new Vector3(i, this.currentLayer.Value, WorldHelpers.MapSize) +  new Vector3(currentChunck.Value.x, 0, currentChunck.Value.y) * WorldHelpers.MapSize;
                Gizmos.DrawLine( startPos, endPos);
            }
            for (int i = 0; i < WorldHelpers.MapSize + 1; i++)
            {
                Vector3 startPos = new Vector3(0, this.currentLayer.Value, i ) +  new Vector3(currentChunck.Value.x, 0, currentChunck.Value.y) * WorldHelpers.MapSize;
                Vector3 endPos = new Vector3( WorldHelpers.MapSize, this.currentLayer.Value, i )  + new Vector3(currentChunck.Value.x, 0, currentChunck.Value.y) * WorldHelpers.MapSize;
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
}