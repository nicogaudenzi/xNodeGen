using UnityEngine;
using UnityEngine.Events;
namespace Hexiled.World.Events
{
    [CreateAssetMenu(fileName = "Vector2IntEventSO", menuName = "Hexiled/Events/Vector2IntEventSO")]

    public class Vector2IntEventSO : ScriptableObject
    {
        public UnityEvent<Vector2Int> Event = new UnityEvent<Vector2Int>();
    }
}