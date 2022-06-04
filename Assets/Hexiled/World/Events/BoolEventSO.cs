using UnityEngine;
using UnityEngine.Events;
namespace Hexiled.World.Events
{
    [CreateAssetMenu(fileName = "BoolEventSO", menuName = "Hexiled/Events/BoolEventSO")]

    public class BoolEventSO : ScriptableObject
    {
        public UnityEvent<bool> Event = new UnityEvent<bool>();
    }
}