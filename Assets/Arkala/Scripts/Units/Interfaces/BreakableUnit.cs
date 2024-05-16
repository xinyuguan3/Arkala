using ClickNext.Scripts.Items;
using UnityEngine;

namespace ClickNext.Scripts.Units.Interfaces
{
    public abstract class BreakableUnit : LocationUnit
    {
        [SerializeField] RawMaterial rawMaterial;
        [SerializeField] int spawnCount;

        protected new void Awake()
        {
            rawMaterial.gameObject.SetActive(false);
            base.Awake();
        }

        public override void ReceiveValue(float value, Vector3 position)
        {
            Current.hp -= value;

            base.ReceiveValue(value, position);

            if (Current.hp <= 0)
            {
                for (int i = 0; i < spawnCount; ++i)
                    SpawnMaterial(rawMaterial);

                Destroy(gameObject);
            }
        }

        protected abstract void SpawnMaterial(RawMaterial rawMaterial);
    }
}