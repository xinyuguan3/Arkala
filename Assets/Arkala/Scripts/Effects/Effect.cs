using System.Collections;
using UnityEngine;

namespace ClickNext.Scripts.Effects
{
    [RequireComponent(typeof(ParticleSystem))]

    public class Effect : MonoBehaviour
    {
        public void Play()
        {
            var particle = GetComponent<ParticleSystem>();
            var main = particle.main;
                main.playOnAwake = false;

            transform.SetParent(null, true);
            gameObject.SetActive(true); 
            StartCoroutine(Spawn(particle));
        }

        private IEnumerator Spawn(ParticleSystem particle)
        {
            yield return new WaitForEndOfFrame();

            particle.Play();
            var duration = particle.main.duration;
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
