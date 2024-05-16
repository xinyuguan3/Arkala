using ClickNext.Scripts.Items;
using ClickNext.Scripts.Units.Interfaces;
using UnityEngine;

namespace ClickNext.Scripts.Units
{
    public class Rock : BreakableUnit
    {
        protected override void SpawnMaterial(RawMaterial rawMaterial)
        {
            var location = _locations[Random.Range(0, _locations.Length)];
            var item = Instantiate(rawMaterial, location.transform, true);
                item.transform.position = location.transform.position;
                item.transform.SetParent(null);
                item.gameObject.SetActive(true);
        }

        public override void SetAllow(bool value) =>
            SetLocationTags(value ? TagType.Rock : TagType.Untagged);
    }
}
