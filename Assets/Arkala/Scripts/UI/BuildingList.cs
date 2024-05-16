using System;
using UnityEngine;

namespace ClickNext.Scripts.UI
{
    [Serializable]
    public struct BuildingList
    {
        public string buildingName;
        public Blueprint prefab;
        public Sprite sprite;
        public Ingredient[] ingredient;
    }
}
