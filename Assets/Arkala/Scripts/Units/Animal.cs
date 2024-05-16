using ClickNext.Scripts.Items;
using ClickNext.Scripts.Units.Interfaces;
using System;
using UnityEngine;
namespace ClickNext.Scripts.Units
{
    [DisallowMultipleComponent]
    public class Animal : MoveUnit
    {
        public event Action<Vector3> OnTakeDamage = delegate { };

        [SerializeField] RawMaterial rawMaterial;
        [SerializeField] int spawnCount;

        private void Start()
        {
            if(rawMaterial)
                rawMaterial.gameObject.SetActive(false);
        }

        public override void SetAllow(bool value) =>
            tag = value ? TagType.Animal.ToString() : TagType.Untagged.ToString();

        public override void ReceiveValue(float value, Vector3 attackerPosition)
        {
            Current.hp -= value;

            base.ReceiveValue(value, attackerPosition);

            if (Current.hp <= 0)
            {
                for (int i = 0; i < spawnCount; ++i)
                    SpawnMaterial(rawMaterial);

                Destroy(gameObject);
            }
            else
            {
                OnTakeDamage(attackerPosition);
            }
        }

        protected void SpawnMaterial(RawMaterial rawMaterial)
        {
            if (!rawMaterial)
                return;

            var item = Instantiate(rawMaterial, transform, true);
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
        }
    }
}
