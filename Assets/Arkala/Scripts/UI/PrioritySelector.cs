using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI
{
    public class PrioritySelector : MonoBehaviour
    {
        [SerializeField] Button upButton;
        [SerializeField] Button downButton;
        [SerializeField] Button hideButton;
        [SerializeField] TMP_Text selectedDisplay;

        private int value;
        private Action<int> callback;

        private void Awake()
        {
            upButton.onClick.AddListener(() => SetPrioritiy(1));
            downButton.onClick.AddListener(() => SetPrioritiy(-1));
            hideButton.onClick.AddListener(Hide);
        }

        public void Show(int value, Vector3 position, Action<int> callback)
        {
            this.value = value;
            this.callback = callback;
            transform.position = position;

            SetDisplay();
            gameObject.SetActive(true);
        }

        public void Hide() =>
            gameObject.SetActive(false);

        private void SetPrioritiy(int increament)
        {
            value += increament;
            value = Mathf.Clamp(value, 0, 9);

            SetDisplay();
        }

        private void SetDisplay()
        {
            selectedDisplay.text = value.ToString();
            callback(value);
        }
    }
}
