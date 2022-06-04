using UnityEngine;
using Hexiled.World.SO;

namespace Hexiled.World.Components
{
    public class ChunckIndicator : MonoBehaviour
    {
        [SerializeField]
        //[HideInInspector]
        Vector2IntSO currentChunck;

        [SerializeField]
        //[HideInInspector]
        WorldDataContainer wdc;


        private void OnDrawGizmosSelected()
        {

            int currentLayer = 0;

            Gizmos.color = Color.blue;
            foreach (Vector2 v in wdc.WorldData.WorldChuncks.Keys)
            {
                //if (v == currentChunck.Value)
                //    continue;
                Vector3 _pos = new Vector3(v.x, 0, v.y) * 32;
                Gizmos.DrawLine(_pos + new Vector3(0, currentLayer, 0) , _pos + new Vector3(32, currentLayer, 0) );
                Gizmos.DrawLine(_pos + new Vector3(0, currentLayer, 0) , _pos + new Vector3(0, currentLayer,32) );
                Gizmos.DrawLine(_pos + new Vector3(32, currentLayer, 0) , _pos + new Vector3(32, currentLayer,32) );
                Gizmos.DrawLine(_pos + new Vector3(0, currentLayer, 32) , _pos + new Vector3(32, currentLayer, 32) );

            }
            Gizmos.color = Color.cyan;
            foreach (Vector2 v in wdc.WorldData.TerrainChunkHolder.Keys)
            {
                //if (v == currentChunck.Value)
                //    continue;
                Vector3 _pos = new Vector3(v.x, 0.1f, v.y) * 32;
                Gizmos.DrawLine(_pos + new Vector3(0, currentLayer, 0) , _pos + new Vector3(32, currentLayer, 0) );
                Gizmos.DrawLine(_pos + new Vector3(0, currentLayer, 0) , _pos + new Vector3(0, currentLayer, 32) );
                Gizmos.DrawLine(_pos + new Vector3(32, currentLayer, 0) , _pos + new Vector3(32, currentLayer, 32) );
                Gizmos.DrawLine(_pos + new Vector3(0, currentLayer, 32) , _pos + new Vector3(32, currentLayer, 32) );

            }
        }
    }
}