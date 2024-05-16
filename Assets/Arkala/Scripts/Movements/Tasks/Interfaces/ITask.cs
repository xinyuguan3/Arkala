using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Units;
using System;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks.Interfaces
{
    public abstract class ITask : BaseMovement
    {
        public event Action OnDone = delegate { };

        [Range(0, 10)] public int chanceValue;

        protected Weapon _weapon;
        protected Worker _worker;
        protected WorkerAnimation _workerAnimation;

        protected new void Awake()
        {
            _worker = GetComponent<Worker>();
            _workerAnimation = GetComponent<WorkerAnimation>();

            base.Awake();
        }

        public abstract bool IsAvailable();

        public bool StartWork()
        {
            enabled = IsAvailable();
            return enabled;
        }

        protected void OnEnable()
        {
            if(_weapon)
                _worker.EquipItem(_weapon);
        }

        protected void OnDisable()
        {
            if(_weapon)
                _worker.UnEquipItem(_weapon);

            _workerAnimation.SetDefault();

            StopAllCoroutines();
            OnDone();
        }
    }
}
