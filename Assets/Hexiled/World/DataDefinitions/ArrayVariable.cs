
using UnityEngine;
namespace Hexiled.World.SO
{
    public class ArrayVariable<T> : ScriptableObject
    {
        [SerializeField]
        T[] value;

        public T[] Value { get => value; set => this.value = value; }
    }

}