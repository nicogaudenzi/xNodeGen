using UnityEngine;
using UnityEngine.Events;
namespace Hexiled.World.Events
{
    [CreateAssetMenu(fileName = "FloatEventSO", menuName = "Hexiled/Events/FloatEventSO")]

    public class FloatEventSO : ScriptableObject
    {
        public UnityEvent<float> Event = new UnityEvent<float>();
    }
}