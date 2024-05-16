using ClickNext.Scripts.Units;
using ClickNext.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class BlueprintManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button placeButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button cancelButton;

        [Header("Grid")]
        [SerializeField] private float tileLength;
        [SerializeField] private GameObject grid;
        [SerializeField] private GameObject gridDisplay;
        [SerializeField] private Transform buildingRoot;

        private RaycastManager raycastManager;
        private CameraMover cameraMover;
        private Blueprint currentBlueprint;
        private Ingredient[] ingredients;
        private bool isDragged;

        void Awake()
        {
            raycastManager = FindObjectOfType<RaycastManager>();
            cameraMover = FindObjectOfType<CameraMover>();

            placeButton.onClick.AddListener(OnPlace);
            rotateButton.onClick.AddListener(OnRotate);
            cancelButton.onClick.AddListener(OnCancel);

            panel.gameObject.SetActive(false);
            grid.gameObject.SetActive(false);
        }

        public void ShowMenu(Blueprint prefab, Ingredient[] ingredients)
        {
            if (currentBlueprint)
                return;

            this.ingredients = ingredients;
            currentBlueprint = Instantiate(prefab, grid.transform);
            currentBlueprint.transform.localPosition = Vector3.zero;
            panel.gameObject.SetActive(true);

            if (grid)
                grid.SetActive(true);

            // set layers.
            var blueprintLayer = LayerMask.NameToLayer(LayerType.Blueprint.ToString());
            gridDisplay.layer = blueprintLayer;
            currentBlueprint.gameObject.layer = blueprintLayer;
            raycastManager.SetLayerMask(LayerMask.GetMask(LayerType.Blueprint.ToString()));
        }

        private void OnCancel()
        {
            var storage = FindObjectOfType<Storage>();
            if (storage)
            {
                foreach (var each in ingredients)
                    storage.AddRawMaterial(each.type, each.value);
            }

            DestroyBlueprint();
        }

        private void OnPlace()
        {
            currentBlueprint.StartBuild(buildingRoot);
            DestroyBlueprint();
        }

        private void DestroyBlueprint()
        {
            Destroy(currentBlueprint.gameObject);
            panel.gameObject.SetActive(false);

            if (grid)
                grid.SetActive(false);

            raycastManager.SetLayerMask(Physics.AllLayers);
        }

        private void OnRotate()
        {
            currentBlueprint.transform.eulerAngles += (Vector3.up * 90f);
        }

        private void OnEnable()
        {
            raycastManager.OnBegan += OnBegan;
            raycastManager.OnDrag += OnDrag;
            raycastManager.OnEnd += OnEnd;
        }

        private void OnDisable()
        {
            raycastManager.OnBegan -= OnBegan;
            raycastManager.OnDrag -= OnDrag;
            raycastManager.OnEnd -= OnEnd;
        }

        private void OnEnd()
        {
            if (!currentBlueprint)
                return;

            SetDrag(false);
        }
        private void OnBegan(GameObject hitObject, Vector3 position)
        {
            if (!currentBlueprint)
                return;

            if (hitObject == currentBlueprint.gameObject)
                SetDrag(true);
        }

        private void SetDrag(bool value)
        {
            currentBlueprint.HideCollider(value);
            cameraMover.enabled = !value;
            isDragged = value;
        }

        private void OnDrag(GameObject hitObject, Vector3 position)
        {
            if (hitObject == gridDisplay && isDragged)
            {
                currentBlueprint.transform.position = new Vector3(position.x, 0f, position.z);

                var location = currentBlueprint.transform.localPosition;
                var x = Mathf.CeilToInt(location.x / tileLength) * tileLength;
                var z = Mathf.CeilToInt(location.z / tileLength) * tileLength;
                currentBlueprint.transform.localPosition = new Vector3(x, 0f, z);
            }
        }

        private void Update()
        {
            if (currentBlueprint && panel.activeInHierarchy)
                panel.transform.position = RaycastManager.CurrentCamera.WorldToScreenPoint(currentBlueprint.MenuLocation);
        }
    }
}
