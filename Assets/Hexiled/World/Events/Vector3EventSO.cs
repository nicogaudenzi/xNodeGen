using UnityEngine;
using UnityEngine.Events;
namespace Hexiled.World.Events
{
    [CreateAssetMenu(fileName = "Vector3EventSO", menuName = "Hexiled/Events/Vector3EventSO")]

    public class Vector3EventSO:ScriptableObject
    {
        public UnityEvent<Vector3> Event = new UnityEvent<Vector3>();
    }
}