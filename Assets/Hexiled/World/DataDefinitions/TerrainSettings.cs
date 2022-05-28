using UnityEngine;

namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "Terrain Settings", menuName = "Hexiled/Yeti3D/Settings/TerrainSettings")]
    public class TerrainSettings : ScriptableObject
    {

        [SerializeField]
        public Gradient gradient;
        public float heightMultiplier=1;
        public AnimationCurve heightCurve;
        public float noiseScale=64;
        public int octaves=4;
        public float lacunarity=1;
        [Range(0, 1)]
        public float persistance=.3f;
        public int seed=1;
        //public Vector2 offset;
        private void OnEnable()
        {
            if (gradient == null)
                gradient = new Gradient();
            if (heightCurve == null)
            {
               
                heightCurve = AnimationCurve.EaseInOut(0,0,1,1);
            }

        }
        //public Material terrainMat;

        //public bool useTerrainInRuntime;
        //public bool terrainIsWalkable;

        //public bool renderWater;
        //public bool waterColliders;
        //public int waterKey;
        //public bool useWaterPlane;

    }
}