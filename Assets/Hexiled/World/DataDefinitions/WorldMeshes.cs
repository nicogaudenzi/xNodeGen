using UnityEngine;
using UnityEngine.Events;

namespace Hexiled.World.SO
{
    
    [CreateAssetMenu(fileName = "World Mesh Container", menuName = "Hexiled/Yeti3D/Containers/WorldMesh")]
    public class WorldMeshes: ScriptableObject
    {
        [SerializeField]
        GameObject[] meshes;
        public GameObject[] Meshes { get => meshes; set => meshes = value; }
        [SerializeField]
        UnityEvent MeshesChanged;
        [HideInInspector]
        public Texture2D PreviewIcon;
        private void OnEnable()
        {
            if (meshes == null)
            {
                meshes = new GameObject[1];
            }
            if (MeshesChanged == null)
                MeshesChanged = new UnityEvent();
        }

        void OnValidate()
        {
            MeshesChanged.Invoke();
        }
    }
}