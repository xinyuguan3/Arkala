using UnityEngine;
namespace ClickNext.Scripts
{
    public enum SlotType : int { back, leftHand, rightHand, carry }

    [DisallowMultipleComponent]
    public class Slot : MonoBehaviour
    {
        public SlotType slotType;
    }
}
