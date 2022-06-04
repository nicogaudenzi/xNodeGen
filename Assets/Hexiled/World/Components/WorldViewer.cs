using UnityEngine;
using Hexiled.World.Events;
using Hexiled.World.SO;
namespace Hexiled.World.Components
{
    [ExecuteInEditMode]
    public class WorldViewer : MonoBehaviour
    {
        //[SerializeField]
        //Vector3Int offset;
        [SerializeField]
        Vector2IntSO selectedChunk;
        [SerializeField]
        Vector3EventSO chunckChanged;
        
        private void OnEnable()
        {
            //if(selectedChunk==null)
            //    selectedChunk = (Vector2IntSO)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables+"SelectedChunk"+".asset");
            //if(chunckChanged==null)
            //    chunckChanged = (Vector3Event)EditorGUIUtility.LoadRequired(InternalPaths.vector3events+"ChunkChanged"+".asset");
            chunckChanged.Event.AddListener(ChangePosition);
            ChangePosition(Vector3.zero);
        }
        private void OnDisable()
        {
            chunckChanged.Event.RemoveListener(ChangePosition);

        }
        private void OnDestroy()
        {
            chunckChanged.Event.RemoveListener(ChangePosition);
        }
        void ChangePosition(Vector3 v)
        {
            Vector2 newpos = selectedChunk.Value * WorldHelpers.MapSize;
            gameObject.transform.position = new Vector3(newpos.x,0,newpos.y);
        }
        

    }
}