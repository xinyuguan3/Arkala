using ClickNext.Scripts.Units.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace ClickNext.Scripts.Items.Weapons
{
    public enum Tools: int
    {
        Hatchet,
        Hammer,
        Pot,
        Pickaxe
    }

    [DisallowMultipleComponent]
    public class Tool : Weapon
    {
        public Tools Type { get { return type; } }
        [SerializeField] Tools type;

        public override IEnumerator DelayAttack(Unit target, Action onAttack, Action<float> onHit)
        {
            onAttack();
            yield return new WaitForSeconds(_animationDelay);
            onHit(_damage);
        }
    }
}
