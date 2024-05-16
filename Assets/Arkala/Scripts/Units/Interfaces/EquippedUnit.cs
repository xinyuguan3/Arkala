using ClickNext.Scripts.Items;
using ClickNext.Scripts.Items.Weapons;
using System.Linq;
using UnityEngine;

namespace ClickNext.Scripts.Units.Interfaces
{
    public abstract class EquippedUnit : MoveUnit
    {
        [SerializeField] private Slot[] _slots;

        protected new void Awake()
        {
            _slots = GetComponentsInChildren<Slot>();
            base.Awake();
        }

        public void EquipItem(Item item)
        {
            item.gameObject.SetActive(true);

            if (item is RawMaterial)
                Equip(SlotType.carry, item);

            else if (item is Weapon) 
            {
                var weapon = item as Weapon;
                foreach (var each in weapon.parts)
                    Equip(each.slotType, each.part);
            }

            UpdateStatus();
        }

        private void Equip(SlotType slotType, Item item)
        {
            var equipSlot = GetSlot(slotType);
            if (equipSlot != null)
                SetHierarchy(item.transform, equipSlot);
        }

        public void UnEquipItem(Item item)
        {
            if (item is Weapon)
            {
                var weapon = item as Weapon;
                foreach (var each in weapon.parts)
                    SetHierarchy(each.part.transform, weapon.transform);

                if(gameObject.activeInHierarchy)
                    weapon.transform.SetParent(transform);
            }

            item.gameObject.SetActive(false);

            UpdateStatus();
        }

        private Transform GetSlot(SlotType slotType)
        {
            var slot = _slots.FirstOrDefault(each => each.slotType == slotType);
            return slot ? slot.transform : null;
        }

        public void UpdateStatus()
        {
            var items = GetComponentsInChildren<Item>(includeInactive: false);

            float weight = 0;
            float range = 0;
            float actionDelay = 0;
            foreach (var each in items)
            {
                weight += each.weight;
                range += each.range;
                actionDelay += each.actionDelay;
            }

            Current.speed = Speed - weight;
            Current.range = this.Range + range;
            Current.actionDelay = this.ActionDelay + actionDelay;
        }

        private void SetHierarchy(Transform child, Transform parent)
        {
            if (!gameObject.activeInHierarchy)
                return;

            child.SetParent(parent);
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;
        }
    }
}
