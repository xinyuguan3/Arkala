using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.Units;
using ClickNext.Scripts.Units.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class MainManager : MonoBehaviour
    {
        [Serializable]
        private struct Panel
        {
            public Button openButton;
            public IManager panel;
        }

        [SerializeField] private Button exitButton;
        [SerializeField] private Panel[] panels;

        private RaycastManager raycastManager;

        protected void Awake()
        {
            raycastManager = FindObjectOfType<RaycastManager>();
            exitButton.onClick.AddListener(Exit);

            HidePanels();

            foreach (var each in panels)
            {
                each.openButton.onClick.AddListener(() =>
                {
                    HidePanels();
                    each.panel.OpenDialog(true);
                    each.panel.transform.SetAsLastSibling();
                    // each.openButton.transform.SetAsLastSibling();
                });
            }

            ShowDefaultMenu(true);
        }

        private void OnEnable() => raycastManager.OnHit += OnHit;
        private void OnDisable() => raycastManager.OnHit -= OnHit;

        private void OnHit(GameObject hitObject, Vector3 position)
        {
            var unit = hitObject.GetComponent<Unit>();
            if (unit)
            {
                GoIn(unit);
            }
        }

        //go in building.
        private void GoIn(Unit unit)
        {
            var building = unit as Building;
            if (building && building.CameraView)
            {
                ShowDefaultMenu(false);
                raycastManager.SetRaycastCamera(building.CameraView);
            }
        }

        //exit to outside.
        private void Exit()
        {
            ShowDefaultMenu(true);
            raycastManager.SetDefaultCamera();
        }

        private void ShowDefaultMenu(bool value)
        {
            foreach (var each in panels)
            {
                each.panel.OpenDialog(false);
                each.openButton.gameObject.SetActive(value);
            }

            exitButton.gameObject.SetActive(!value);
        }

        public void HidePanels()
        {
            foreach (var each in panels)
                each.panel.OpenDialog(false);
        }
    }
}
