using UnityEngine;
using Hexiled.World.SO;
using Hexiled.World.Events;
using UnityEditor;
namespace Hexiled.World.Components
{
    [ExecuteInEditMode]
    public class TurnoffNeighbours : MonoBehaviour
    {
        [SerializeField]
        BoolEventSO autoUpdateChanged;
        [SerializeField]
        BoolSO terrainVisibility;
        private void OnEnable()
        {
            //if (autoUpdateChanged == null)
            //    autoUpdateChanged = (BoolEvent)EditorGUIUtility.LoadRequired(InternalPaths.boolevents + "AutoUpdateChanged" + ".asset");
            //if (terrainVisibility == null)
            //    terrainVisibility = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "TerrainVisible" + ".asset");

            autoUpdateChanged.Event.AddListener(ToggleActive);
        }
        private void OnDisable() => Clean();
        private void OnDestroy() => Clean();

        void Clean()
        {
            autoUpdateChanged.Event.RemoveListener(ToggleActive);
        }
        void ToggleActive(bool b)
        {
            if (!terrainVisibility.Value) return;
            foreach (Transform t in transform)
            {
                if(t.GetComponent<MeshRenderer>())
                t.GetComponent<MeshRenderer>().enabled = b;
            }
        }

    }
}