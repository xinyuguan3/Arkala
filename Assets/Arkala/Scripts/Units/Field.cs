using ClickNext.Scripts.Units.Interfaces;
using System.Collections;
using UnityEngine;

namespace ClickNext.Scripts.Units
{
    public class Field : StateUnit
    {
        [SerializeField] private float growTime;
        [SerializeField] private GameObject products;

        private GameObject currentProducts;
        private Coroutine growingRoutine;

        private void OnEnable() => OnStateChanged += StateChanged;
        private void OnDisable() => OnStateChanged -= StateChanged;

        private void StateChanged(int stateIndex)
        {
            if (stateIndex == 0)
            {
                SetLocationTags(TagType.Farm);
            }
            else if (stateIndex == _maxState)
            {
                currentProducts = Instantiate(products, transform);
                currentProducts.SetActive(true);
            }
            else
            {
                // wait until growed.
                SetLocationTags(TagType.Untagged);

                if(growingRoutine == null)
                    growingRoutine = StartCoroutine(Growing());
            }
        }

        private IEnumerator Growing()
        {
            yield return new WaitForSeconds(growTime);
            SetLocationTags(TagType.Farm);
            growingRoutine = null;
        }

        private void Start()
        {
            products.SetActive(false);
            _targetTag = TagType.Farm;
            Current.hp = 0f;
            UpdateState(0);
        }

        private new void Update()
        {
            if (IsEmptyField())
                return;

            base.Update();
        }

        private bool IsEmptyField()
        {
            if (_currentStateIndex == _maxState &&
                currentProducts.transform.childCount == 0)
            {
                Destroy(currentProducts);
                currentProducts = null;

                _currentStateIndex = 0;
                Current.hp = 0;

                SetLocationTags(TagType.Farm);
                UpdateState(_currentStateIndex);
                return true;
            }

            return false;
        }

        public override void ReceiveValue(float value, Vector3 position)
        {
            if (growingRoutine != null)
                return;

            base.ReceiveValue(value, position);
        }
    }
}
