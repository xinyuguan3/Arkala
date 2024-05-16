using UnityEngine;

namespace ClickNext.Scripts.Animations
{
    public class UnitAnimation : MonoBehaviour
    {
        public static readonly string Speed = "speed";

        [SerializeField] protected Animator _animator;

        protected bool _isWalking;
        protected Vector2 _previousPosition;

        private Vector2 currentVelocity;

        void Update()
        {
            var position = new Vector2(transform.position.x, transform.position.z);
            currentVelocity = (position - _previousPosition) / Time.deltaTime;
            _previousPosition = position;

            _isWalking = currentVelocity.magnitude > 0.1f;
            _animator.SetFloat(Speed, currentVelocity.magnitude);
        }
    }
}
