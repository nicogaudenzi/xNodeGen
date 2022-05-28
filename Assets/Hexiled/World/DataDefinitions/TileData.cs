using UnityEngine;
namespace Hexiled.World.Data
{
[System.Serializable]
public class TileData
{
    public TileType tileType;
    public MeshType meshType;
    public MeshRot meshRot;
    public MeshSteep meshSteep;
    public bool walkable;
    public float height = 0;
    public int room;
    public int rotation = 0;
    //public int topMaterialIndex,botMaterialIndex;
    //public Material topMat, botMat;
        public int topIndex,botIndex;
    public int meshPrefabIndex=0;
    public float toptileX = float.MinValue, toptileY = float.MinValue, bottileX = float.MinValue, bottileY = float.MinValue;
    [SerializeField]
    protected float uv0x = 1, uv0y = 0, uv1x = 0, uv1y = 0, uv2x = 0, uv2y = 1, uv3x = 1, uv3y = 1;
    [SerializeField]
    protected float buv0x = 1, buv0y = 0, buv1x = 0, buv1y = 0, buv2x = 0, buv2y = 1, buv3x = 1, buv3y = 1;
    [SerializeField]
    public Color color = Color.white;

    public TileData()
    {
        this.tileType = TileType.Empty;
        this.height = 0;
        this.walkable = false;
        this.meshType = MeshType.Ramp;
        this.meshRot = MeshRot.North;
        this.meshSteep = MeshSteep.Full;
        this.room = 0;
        this.rotation = 0;
        //this.topMaterialIndex = 0;
        //this.botMaterialIndex = 0;
        this.toptileX = 0;
        this.toptileY = 0;
        this.bottileX = 0;
        this.bottileY = 0;
        this.color = Color.grey;
            this.meshPrefabIndex = 0;
    }
    public TileData(TileType type)
    {
        this.tileType = type;
    }

    public TileData(TileData t)
    {
        this.tileType = t.tileType;
        this.height = t.height;
        SetTopUVs(t.UVs);
        SetBottomUVs(t.bottomUVs);
        this.walkable = t.walkable;
        this.meshType = t.meshType;
        this.meshRot = t.meshRot;
        this.meshSteep = t.meshSteep;
        this.room = t.room;
        this.rotation = t.rotation;
            this.topIndex = t.topIndex;
            this.botIndex = t.botIndex;
           
            this.toptileX = t.toptileX;
        this.toptileY = t.toptileY;
        this.bottileX = t.bottileX;
        this.bottileY = t.bottileY;
        this.color = t.color;
    }
        
    public Vector2[] UVs
    {
        get
        {
            Vector2[] ret = new Vector2[4];
            ret[0] = new Vector2(uv0x, uv0y);
            ret[1] = new Vector2(uv1x, uv1y);
            ret[2] = new Vector2(uv2x, uv2y);
            ret[3] = new Vector2(uv3x, uv3y);

            return ret;
        }
    }
    public Vector2[] bottomUVs
    {
        get
        {
            Vector2[] ret = new Vector2[4];
            ret[0] = new Vector2(buv0x, buv0y);
            ret[1] = new Vector2(buv1x, buv1y);
            ret[2] = new Vector2(buv2x, buv2y);
            ret[3] = new Vector2(buv3x, buv3y);

            return ret;
        }
    }
    public void SetTopUVs(Vector2[] _uvs){
        uv0x = _uvs[0].x;
        uv0y = _uvs[0].y;

        uv1x = _uvs[1].x;
        uv1y = _uvs[1].y;

        uv2x = _uvs[2].x;
        uv2y = _uvs[2].y;

        uv3x = _uvs[3].x;
        uv3y = _uvs[3].y;
    }
    public void SetBottomUVs(Vector2[] _uvs)
    {
        buv0x = _uvs[0].x;
        buv0y = _uvs[0].y;

        buv1x = _uvs[1].x;
        buv1y = _uvs[1].y;

        buv2x = _uvs[2].x;
        buv2y = _uvs[2].y;

        buv3x = _uvs[3].x;
        buv3y = _uvs[3].y;
    }
}
public enum TileType { Empty, Ground, Mesh }
public enum MeshType {Ramp, Diagonal_Wall,Prefab}
public enum MeshRot {North,West,East,South}
public enum MeshSteep { Quarter, Half, ThreeQuarters, Full }
}