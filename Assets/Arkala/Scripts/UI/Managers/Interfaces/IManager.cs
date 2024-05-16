using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers.Interfaces
{
    public abstract class IManager : MonoBehaviour
    {
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;

        protected void Awake()
        {
            if(openButton)
                openButton.onClick.AddListener(() => OpenDialog(true));

            closeButton.onClick.AddListener(() => OpenDialog(false));
        }

        public virtual void OpenDialog(bool value)
        {
            if(openButton)
                openButton.interactable = !value;

            if(value)
            {
                transform.SetAsLastSibling();

                if(openButton)
                    openButton.transform.SetAsLastSibling();
            }
        }
    }
}
