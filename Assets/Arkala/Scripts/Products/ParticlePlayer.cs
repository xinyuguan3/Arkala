using ClickNext.Scripts.Products.Interfaces;
using UnityEngine;

namespace ClickNext.Scripts.Products
{
    public class ParticlePlayer : IPlayer
    {
        [SerializeField] ParticleSystem particle;
        private void Awake() => StopProcess();
        public override void RunProcess() => particle.Play();

        public override void StopProcess() => particle.Stop();
    }
}
