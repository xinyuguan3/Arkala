using TMPro;
using UnityEngine;

namespace ClickNext.Scripts.UI.Managers.Templates
{
    public class TaggerTemplate : MonoBehaviour
    {
        public TMP_Text DisplayName;
        [HideInInspector] public Transform AttachedWorker;

        void Update()
        {
            if (!AttachedWorker)
                return;

            transform.position = RaycastManager.CurrentCamera.WorldToScreenPoint(AttachedWorker.position);
        }
    }
}
