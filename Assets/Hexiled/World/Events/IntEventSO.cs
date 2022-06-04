using UnityEngine;
using UnityEngine.Events;
namespace Hexiled.World.Events
{
    [CreateAssetMenu(fileName = "IntEventSO", menuName = "Hexiled/Events/IntEventSO")]

    public class IntEventSO : ScriptableObject
    {
        public UnityEvent<int> Event = new UnityEvent<int>();
    }
}