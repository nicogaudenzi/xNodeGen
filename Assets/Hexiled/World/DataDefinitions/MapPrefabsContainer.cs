using UnityEngine;
namespace Hexiled.World.SO {
    [CreateAssetMenu(fileName = "MapPrefabsContainer", menuName = "Hexiled/Yeti3D/Containers/MapPrefabsContainer")]

    public class MapPrefabsContainer : ScriptableObject
    {
        [SerializeField]
        MapPrefab[] mapPrefabs;

        public MapPrefab[] MapPrefabs { get => mapPrefabs; set => mapPrefabs = value; }
    }
}