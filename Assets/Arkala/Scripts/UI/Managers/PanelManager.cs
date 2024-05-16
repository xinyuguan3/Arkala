using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.UI.Managers.Templates;
using ClickNext.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class PanelManager : IManager
    {
        [SerializeField] private Transform layout;
        [SerializeField] private BuildingTemplate buildingTemplate;
        [SerializeField] private BuildingList[] data;

        private BlueprintManager blueprintManager;

        private new void Awake()
        {
            base.Awake();

            blueprintManager = FindObjectOfType<BlueprintManager>();
            buildingTemplate.gameObject.SetActive(false);
            GenerateList();
        }

        private void GenerateList()
        {
            foreach (var each in data)
            {
                var building = Instantiate(buildingTemplate, layout);
                    building.Create(each);
                    building.GetComponent<Button>().onClick.AddListener(()=> OnSelected(each.prefab, each.ingredient));
                    building.gameObject.SetActive(true);
            }
        }

        private void OnSelected(Blueprint prefab, Ingredient[] ingredient)
        {
            var isSuccess = Storage.Withdraw(ingredient);
            if (isSuccess && blueprintManager)
            {
                blueprintManager.ShowMenu(prefab, ingredient);
                OpenDialog(false);
            }
        }

        public override void OpenDialog(bool value)
        {
            gameObject.SetActive(value);
            base.OpenDialog(value);
        }
    }
}
