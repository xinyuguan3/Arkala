using ClickNext.Scripts.Products;
using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.UI.Managers.Templates;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class ProductManager : IManager
    {
        [SerializeField] GameObject panel;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text ordersText;
        [SerializeField] TMP_Text durationText;
        [SerializeField] Image icon;
        [SerializeField] Button orderButton;

        [Header("Ingredients")]
        [SerializeField] IngredientTemplate ingredientTemplate;

        private readonly List<IngredientTemplate> ingredientList = new List<IngredientTemplate>();
        private RaycastManager raycastManager;
        private Producer currentProducer;

        private new void Awake()
        {
            base.Awake();

            raycastManager = FindObjectOfType<RaycastManager>(includeInactive: true);

            panel.SetActive(false);
            ingredientTemplate.gameObject.SetActive(false);

            orderButton.onClick.AddListener(() => currentProducer.Order());
        }

        private void OnEnable() => raycastManager.OnHit += OnSelected;
        private void OnDisable() => raycastManager.OnHit -= OnSelected;

        private void OnSelected(GameObject hit, Vector3 position)
        {
            var producer = hit.GetComponent<Producer>();
            if (producer)
                Show(producer);
        }

        private void Show(Producer producer)
        {
            currentProducer = producer;
            nameText.text = producer.ProductName;
            icon.sprite = Resources.Load<Sprite>($"Icons/{producer.ProductName}");

            GenerateIngredients();
            OpenDialog(true);
        }

        private void GenerateIngredients()
        {
            foreach (var ingredient in ingredientList)
                Destroy(ingredient.gameObject);
            ingredientList.Clear();

            foreach (var each in currentProducer.Ingredients)
            {
                var ingredient = Instantiate(ingredientTemplate, ingredientTemplate.transform.parent);
                    ingredient.Create(each);
                    ingredient.gameObject.SetActive(true);

                ingredientList.Add(ingredient);
            }

            UpdateInfo();
        }

        private void FixedUpdate()
        {
            if (!currentProducer || !panel.activeInHierarchy)
                return;

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            ordersText.text = currentProducer.OrdersCount.ToString();
            durationText.text = $"{currentProducer.RemainTime.ToString("0")}s";
        }

        public override void OpenDialog(bool value)
        {
            panel.SetActive(value);
            base.OpenDialog(value);
        }
    }
}
