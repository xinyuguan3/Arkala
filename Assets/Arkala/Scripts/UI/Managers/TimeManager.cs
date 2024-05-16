using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] Toggle pauseToggle;
        [SerializeField] Toggle playToggle;
        [SerializeField] Toggle forwardToggle;

        private void Awake()
        {
            pauseToggle.onValueChanged.AddListener(ison => ChangePlaySpeed(ison, 0f));
            playToggle.onValueChanged.AddListener(ison => ChangePlaySpeed(ison, 1f));
            forwardToggle.onValueChanged.AddListener(ison => ChangePlaySpeed(ison, 3f));
        }

        private void ChangePlaySpeed(bool ison, float speed)
        {
            if(ison)
                Time.timeScale = speed;
        }
    }
}
