using UnityEngine;

namespace ClickNext.Scripts.Items
{
    [DisallowMultipleComponent]
    public class RawMaterial : Item
    {
        public MaterialType materialType;
        private GameObject owner;

        private void Awake() =>
            tag = TagType.Materials.ToString();

        public bool SetPicked(GameObject owner)
        {
            if (owner == null)
            {
                gameObject.SetActive(true);
                //check active in hierarchy on disable event.
                if (gameObject.activeInHierarchy)
                {
                    transform.SetParent(null);
                    var position = transform.position;
                        position.y = 0f;
                    transform.position = position;
                    transform.eulerAngles = Vector3.zero;
                }
            }

            if (owner && this.owner != null)
                return false;

            this.owner = owner;
            tag = owner? TagType.Untagged.ToString(): TagType.Materials.ToString();

            return true;
        }

        public bool IsOwner(GameObject owner) => this.owner == owner;
    }
}
