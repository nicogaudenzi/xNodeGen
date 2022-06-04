using UnityEngine;
namespace Hexiled.World.Data
{
    [System.Serializable]
    public class SerializableMultiArray<T> where T : new()
    {

        public DataLayer<T>[] data;
        private const int mapSize = WorldHelpers.MapSize;
        [SerializeField]
        private int mapHeight = 1;
        public SerializableMultiArray()
        {
            DataLayer<T>[] _data = new DataLayer<T>[mapHeight];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new DataLayer<T>(MapSize, MapSize);
            }
            data = _data;
        }

        public T this[int i, int j, int k] //This is to fix inversion of the indexes;
        {
            get
            {
                return data[j][k][i];
            }
            set
            {
                data[j][k][i] = value;
            }
        }




        public static int MapSize
        {
            get
            {
                return mapSize;
            }
        }

        public int MapHeight
        {
            get
            {
                return mapHeight;
            }
        }

        public void AddEmptyLayer()
        {
            DataLayer<T>[] _data = new DataLayer<T>[mapHeight + 1];
            for (int i = 0; i < data.Length; i++)
            {
                _data[i] = data[i];
            }
            _data[_data.Length - 1] = new DataLayer<T>(mapSize, mapSize);
            mapHeight++;
            data = _data;
        }

        public void DeleteTopLayer()
        {
            DataLayer<T>[] _data = new DataLayer<T>[mapHeight - 1];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = data[i];
            }
            mapHeight--;
            data = _data;
        }

        public T GetNeighbor(int x, int y, int z, Direction dir)
        {
            Vector3Int checkOffset = offsets[(int)dir];
            Vector3Int neighborCood = new Vector3Int(x + checkOffset.x, y + checkOffset.y, z + checkOffset.z);
            if (neighborCood.x < 0 || neighborCood.x >= mapSize ||
                neighborCood.y < 0 || neighborCood.y >= mapHeight ||
                neighborCood.z < 0 || neighborCood.z >= mapSize)
                return default(T);
            else
                return this[neighborCood.x, neighborCood.y, neighborCood.z];
        }

        public Vector2Int GetNeighborCoords(int x, int y, int z, Direction dir)
        {
            Vector3Int checkOffset = offsets[(int)dir];
            Vector3Int neighborCood = new Vector3Int(x + checkOffset.x, y + checkOffset.y, z + checkOffset.z);
            if (neighborCood.x < 0 || neighborCood.x >= mapSize ||
                neighborCood.z < 0 || neighborCood.z >= mapSize)
                return new Vector2Int(int.MaxValue, int.MaxValue);

            else
                return new Vector2Int(neighborCood.x, neighborCood.z);
        }


        public bool CheckInBounds(int x, int y, int z, Direction dir)
        {
            Vector3Int checkOffset = offsets[(int)dir];
            Vector3Int neighborCood = new Vector3Int(x + checkOffset.x, y + checkOffset.y, z + checkOffset.z);
            if (neighborCood.x < 0 || neighborCood.x >= mapSize ||
                neighborCood.y < 0 || neighborCood.y >= mapHeight ||
                neighborCood.z < 0 || neighborCood.z >= mapSize)
                return false;
            else
                return true;

        }
        Vector3Int[] offsets = {
        new Vector3Int (0, 0, 1),
        new Vector3Int (1, 0, 0),
        new Vector3Int (0, 0, -1),
        new Vector3Int (-1, 0, 0),
        new Vector3Int (0, 1, 0),
        new Vector3Int (0, -1, 0)
    };

    }
    [System.Serializable]
    public class DataLayer<T> where T : new()
    {

        public Cols<T>[] column;
        public DataLayer()
        {
            column = new Cols<T>[32];
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = new Cols<T>(32);
            }
        }
        public DataLayer(int NumberOfColumns, int NumberOfRows)
        {
            column = new Cols<T>[NumberOfColumns];
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = new Cols<T>(NumberOfRows);
            }
        }
        public Cols<T> this[int index]
        {
            get
            {
                return column[index];
            }
            set
            {
                column[index] = value;
            }
        }
        public int Length
        {
            get
            {
                return column.Length;
            }
        }

        public long LongLength
        {
            get
            {
                return column.LongLength;
            }
        }
    }

    [System.Serializable]
    public class Cols<T>where T : new()
    {
        public T[] row;

        public Cols(int NumberOfRows)
        {
            row = new T[NumberOfRows];
            for (int i = 0; i < row.Length; i++)
            {
                row[i] = new T();
            }
        }

        public T this[int index]
        {

            get
            {
                return row[index];
            }
            set
            {
                row[index] = value;
            }
        }

        public int Length
        {
            get
            {
                return row.Length;
            }
        }

        public long LongLength
        {
            get
            {
                return row.LongLength;
            }
        }

    }
  
    public enum Direction { North, East, South, West, Up }
}