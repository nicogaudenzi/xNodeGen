using UnityEngine;
using UnityEngine.Events;
namespace Hexiled.World.Events
{
    [CreateAssetMenu(fileName = "VoidEventSO", menuName = "Hexiled/Events/VoidEventSO")]

    public class VoidEventSO : ScriptableObject
    {
        public UnityEvent Event = new UnityEvent();
    }
}