using ClickNext.Scripts.Units.Interfaces;
using UnityEngine;
namespace ClickNext.Scripts.Units
{
    public class Grass : LocationUnit
    {
        [SerializeField] float regeneratedTime;
        private float currentTime;

        private void Start()
        {
            currentTime = regeneratedTime;
            SetLocationTags(TagType.Grass);
        }

        public override void ReceiveValue(float value, Vector3 position)
        {
            Current.hp -= value;

            base.ReceiveValue(value, position);

            if (Current.hp <= 0)
            {
                Current.hp = 0f;
                SetLocationTags(TagType.Untagged);
            }
        }

        private void Update()
        {
            if (Current.hp > 0)
                return;

            if (currentTime > 0f)
                currentTime -= Time.deltaTime;
            else
            {
                Current.hp = MaxHp;
                currentTime = regeneratedTime;
                SetLocationTags(TagType.Grass);
            }
        }
    }
}
