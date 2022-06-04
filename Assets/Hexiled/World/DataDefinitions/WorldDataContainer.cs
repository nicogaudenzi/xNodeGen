using UnityEngine;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "WorldDataContainer",menuName = "Hexiled /Editor Specific/WorldDataContainer")]
    public class WorldDataContainer : ScriptableObject
    {
        [SerializeField]
        WorldData worldData;
        
        
        public WorldData WorldData
        {
            get {

                return worldData; }
            set {

                worldData = value;
            }
        }
    }
}
