using ClickNext.Scripts.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers.Templates
{
    public class IngredientTemplate : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text quaitity;
        [SerializeField] private Color validColor;
        [SerializeField] private Color invalidColor;

        private Ingredient data;

        public void Create(Ingredient data)
        {
            this.data = data;
            icon.sprite = Resources.Load<Sprite>($"Icons/{data.type.ToString()}");
            quaitity.text = data.value.ToString();
        }

        private void FixedUpdate()
        {
            quaitity.color = Storage.RawMaterials[(int)data.type] >= data.value ?
                    validColor : invalidColor;
        }
    }
}
