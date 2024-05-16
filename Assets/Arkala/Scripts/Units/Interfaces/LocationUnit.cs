using UnityEngine;

namespace ClickNext.Scripts.Units.Interfaces
{
    public abstract class LocationUnit : Unit
    {
        protected Location[] _locations;

        protected new void Awake()
        {
            _locations = GetComponentsInChildren<Location>();

            if (_locations == null || _locations.Length == 0)
                Debug.LogError($"Unit name: {name} has no any locations.");

            base.Awake();
        }

        protected void SetLocationTags(TagType tagType)
        {
            foreach (var location in _locations)
            {
                location.tag = tagType.ToString();
                location.ParentRoot = gameObject;
            }
        }
    }
}
