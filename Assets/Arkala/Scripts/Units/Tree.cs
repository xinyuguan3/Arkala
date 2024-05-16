using ClickNext.Scripts.Items;
using ClickNext.Scripts.Units.Interfaces;

namespace ClickNext.Scripts.Units
{
    public class Tree : BreakableUnit
    {
        protected override void SpawnMaterial(RawMaterial rawMaterial)
        {
            var item = Instantiate(rawMaterial, transform, true);
                item.transform.SetParent(null);
                item.gameObject.SetActive(true);
        }

        public override void SetAllow(bool value) => 
            SetLocationTags(value ? TagType.Tree : TagType.Untagged);
    }
}
