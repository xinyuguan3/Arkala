using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers.Templates
{
    public class BuildingTemplate : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image icon;
        [SerializeField] private GameObject noRequirement;
        [SerializeField] IngredientTemplate ingredientTemplate;

        public void Create(BuildingList data)
        {
            title.text = data.buildingName;
            icon.sprite = data.sprite;

            ingredientTemplate.gameObject.SetActive(false);
            noRequirement.SetActive(data.ingredient.Length == 0);

            foreach (var each in data.ingredient)
            {
                var ingredient = Instantiate(ingredientTemplate, ingredientTemplate.transform.parent);
                    ingredient.Create(each);
                    ingredient.gameObject.SetActive(true);
            }
        }
    }
}
