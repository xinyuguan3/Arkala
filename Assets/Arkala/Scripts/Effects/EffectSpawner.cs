using UnityEngine;

namespace ClickNext.Scripts.Effects
{
    public class EffectSpawner : MonoBehaviour
    {
        [SerializeField] Effect[] effects;

        //this function has been called by animation event.
        public void Spawn(int index)
        {
            if (effects.Length > index)
            {
                var template = effects[index];
                if (template)
                {
                    var particle = Instantiate(template, transform, true);
                        particle.Play();
                }
            }
        }
    }
}
