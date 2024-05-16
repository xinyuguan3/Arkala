using UnityEngine;

namespace ClickNext.Scripts.Animations
{
    public class WorkerAnimation : UnitAnimation
    {
        public static readonly string CARRYING = "isCarrying";
        public static readonly string IDLE = "idle";
        public static readonly string ATTACK = "attack";
        public static readonly string PULL = "pull";
        public static readonly string CUT = "cut";
        public static readonly string MINING = "mining";
        public static readonly string WATERING = "watering";
        public static readonly string BUILD = "build";

        [SerializeField] private RuntimeAnimatorController _workerController;
        [SerializeField] private RuntimeAnimatorController _hunterController;
        [SerializeField] private RuntimeAnimatorController _cuttingController;
        [SerializeField] private float defaultIdleTime = 14f;

        private float _idleTime;

        private void OnEnable()
        {
            _previousPosition = new Vector2(transform.position.x, transform.position.z);
            _idleTime = defaultIdleTime;
        }

        private void FixedUpdate()
        {
            if (_isWalking)
            {
                _idleTime = defaultIdleTime;
                return;
            }

            _idleTime = Mathf.Clamp(_idleTime - Time.fixedDeltaTime, 0, defaultIdleTime);
            if (_idleTime == 0)
            {
                _idleTime = defaultIdleTime;
                _animator.SetTrigger(IDLE);
            }
        }

        public void SetHunting() => _animator.runtimeAnimatorController = _hunterController;
        public void SetTool() => _animator.runtimeAnimatorController = _cuttingController;
        public void SetDefault() => _animator.runtimeAnimatorController = _workerController;
        public void SetCarrying(bool value) 
        { 
            if(_animator.runtimeAnimatorController == _workerController)
                _animator.SetBool(CARRYING, value); 
        }

        public void PlayAction(string actionName) => _animator.SetTrigger(actionName);
    }
}
