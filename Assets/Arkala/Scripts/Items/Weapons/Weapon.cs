using ClickNext.Scripts.Units.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace ClickNext.Scripts.Items.Weapons
{
    public enum Weapons : int
    {
        Bow
    }

    [DisallowMultipleComponent]
    public abstract class Weapon : Item
    {
        [SerializeField] protected float _damage;
        [SerializeField] protected float _animationDelay;

        [Serializable]
        public struct Part
        {
            public Item part;
            public SlotType slotType;
        }

        public Part[] parts;

        public abstract IEnumerator DelayAttack(Unit target, Action onAttack, Action<float> onHit);
    }
}