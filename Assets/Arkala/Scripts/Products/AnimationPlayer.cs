using ClickNext.Scripts.Products.Interfaces;
using UnityEngine;
namespace ClickNext.Scripts.Products
{
    public class AnimationPlayer : IPlayer
    {
        [SerializeField] Animator animator;

        private void Awake() => StopProcess();
        public override void RunProcess() => animator.enabled = true;
        public override void StopProcess() => animator.enabled = false;
    }
}
