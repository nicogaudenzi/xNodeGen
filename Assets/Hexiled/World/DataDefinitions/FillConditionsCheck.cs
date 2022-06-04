using UnityEngine;
namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "FillConditionsCheck", menuName = "Hexiled/Editor Specific/Fill Conditions Check")]
    public class FillConditionsCheck : ScriptableObject
    {
        [SerializeField]
        bool checktop, checkBottom, checkHeight, checkRotation, checkWalkable, checkRoom, checkTint;
        public bool Checktop { get => checktop; set => checktop = value; }
        public bool CheckBottom { get => checkBottom; set => checkBottom = value; }
        public bool CheckHeight { get => checkHeight; set => checkHeight = value; }
        public bool CheckRotation { get => checkRotation; set => checkRotation = value; }
        public bool CheckWalkable { get => checkWalkable; set => checkWalkable = value; }
        public bool CheckRoom { get => checkRoom; set => checkRoom = value; }
        public bool CheckTint { get => checkTint; set => checkTint = value; }
    }
}
