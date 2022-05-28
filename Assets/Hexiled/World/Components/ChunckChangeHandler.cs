using UnityEngine;
using Hexiled.ProceduralMeshes.Components;
namespace Hexiled.World.Components { 
    [ExecuteInEditMode]
    public class ChunckChangeHandler : MonoBehaviour{
        public void OnEnable()
        {
            EnableEvents();
        }
        public void EnableEvents() {
            foreach (ProceduralMesh pm in transform.parent.GetComponentsInChildren<ProceduralMesh>())
            {
                if (pm.chunkChanged != null)
                {
                    pm.chunkChanged.Event.AddListener(pm.OnChunckChanged);
                }
                if (pm.graphChanged != null)
                {
                    pm.graphChanged.Event.AddListener(pm.GenerateMeshFromEditor);
                }
            }
        }
    }
}