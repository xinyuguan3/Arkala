using TMPro;
using UnityEngine;

namespace ClickNext.Scripts.Utils
{
    public class Framerate : MonoBehaviour
    {
        [SerializeField] TMP_Text fps;
        void Awake() => Application.targetFrameRate = 60;

        private float framerateCount = 0f;
        private float time = 0f;
        void Update()
        {
            if (time >= 1f)
            {
                fps.text = framerateCount.ToString();
                time = 0f;
                framerateCount = 0f;
            }

            framerateCount++;
            time += Time.unscaledDeltaTime;
        }
    }
}
