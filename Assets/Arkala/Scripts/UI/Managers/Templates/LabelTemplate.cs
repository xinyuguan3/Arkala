using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers.Templates
{
    public class LabelTemplate : MonoBehaviour
    {
        [SerializeField] private Image icon;

        private Transform attachedTag;

        public void SetTask(string item, Transform attachedTag)
        {
            icon.sprite = Resources.Load<Sprite>($"Tools/{item}");
            this.attachedTag = attachedTag;
            SetPosition();
        }

        void Update() => SetPosition();

        private void SetPosition()
        {
            if (attachedTag)
                transform.position = RaycastManager.CurrentCamera.WorldToScreenPoint(attachedTag.position);
        }
    }
}
