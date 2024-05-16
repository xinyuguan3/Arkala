using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private Button turnLeftButton;
        [SerializeField] private Button turnRightButton;
        [SerializeField] private Transform rotateTarget;
        [SerializeField] private float rotateSpeed;

        private Quaternion quaternionTarget;

        private void Awake()
        {
            turnLeftButton.onClick.AddListener(TurnLeft);
            turnRightButton.onClick.AddListener(TurnRight);
        }

        private void TurnRight()
        {
            quaternionTarget = rotateTarget.rotation;
            quaternionTarget.eulerAngles += new Vector3(0f,-90f,0f);
        }

        private void TurnLeft()
        {
            quaternionTarget = rotateTarget.rotation;
            quaternionTarget.eulerAngles += new Vector3(0f, 90f, 0f);
        }

        private void Update()
        {
            rotateTarget.rotation = Quaternion.Slerp(rotateTarget.rotation, quaternionTarget, Time.unscaledDeltaTime * rotateSpeed);
        }
    }
}
