using ClickNext.Scripts.Units.Interfaces;
using System;
using UnityEngine;

namespace ClickNext.Scripts.Units
{
    public class Storage : LocationUnit
    {
        public static int[] RawMaterials { private set; get; }

        public Storage()
        {
            var count = Enum.GetValues(typeof(MaterialType)).Length;
            RawMaterials = new int[count];
        }

        private void Start() => SetLocationTags(TagType.Storage);

        public void AddRawMaterial(MaterialType materialType, int value) => 
            RawMaterials[(int)materialType] += value;

        public override void ReceiveValue(float value, Vector3 position){ }

        public static bool Withdraw(Ingredient[] ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                var type = (int)ingredient.type;
                var count = RawMaterials[type];
                var remain = count - ingredient.value;
                if (remain < 0)
                    return false;
            }

            foreach(var ingredient in ingredients)
                RawMaterials[(int)ingredient.type] -= ingredient.value;

            return true;
        }
    }
}
