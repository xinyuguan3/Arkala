using System;
using UnityEngine;

namespace ClickNext.Scripts.Units.Interfaces
{
    [SelectionBase]
    public class StateUnit : LocationUnit
    {
        protected event Action<int> OnStateChanged = delegate { };

        [SerializeField] protected GameObject[] states;

        protected int _currentStateIndex;
        protected int _maxState;
        protected float _stepHp;
        protected TagType _targetTag;

        protected new void Awake()
        {
            _maxState = states.Length - 1;
            _stepHp = MaxHp / _maxState;
            base.Awake();
        }

        protected void Update()
        {
            Current.hp = Mathf.Clamp(Current.hp, 0f, MaxHp);

            if (Current.hp >= (_currentStateIndex + 1) * _stepHp)
            {
                _currentStateIndex = Mathf.Clamp(++_currentStateIndex, 0, _maxState);
                UpdateState(_currentStateIndex);
            }
            else if (Current.hp < _currentStateIndex * _stepHp)
            {
                _currentStateIndex--;
                if (_currentStateIndex < 0)
                    Destroy(gameObject);
                else
                    UpdateState(_currentStateIndex);
            }
        }

        protected void UpdateState(int updatedState)
        {
            _currentStateIndex = updatedState;

            foreach (var state in states)
                state.SetActive(false);

            states[_currentStateIndex].SetActive(true);

            if (_currentStateIndex < _maxState)
                SetLocationTags(_targetTag);
            else
                SetLocationTags(TagType.Untagged);

            OnStateChanged(_currentStateIndex);
        }

        public override void ReceiveValue(float value, Vector3 position)
        {
            var stepHp = (_currentStateIndex + 1) * _stepHp;
            Current.hp += value;
            Current.hp = Current.hp >= stepHp ? stepHp : Current.hp;

            base.ReceiveValue(value, position);
        }
    }
}
