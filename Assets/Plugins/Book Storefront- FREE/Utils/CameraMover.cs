using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClickNext.Scripts.Utils
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] Transform root;
        [SerializeField] Transform defaultLocation;
        [SerializeField] float minX;
        [SerializeField] float maxX;
        [SerializeField] float minZ;
        [SerializeField] float maxZ;

        private Vector3 startTouchPosition;
        private Vector3 currentPosition;

        private bool isStartOverUI;

        private void Awake() => SetDefaultLocation();

        private void SetDefaultLocation()
        {
            root.localPosition = defaultLocation.localPosition;
            currentPosition = root.localPosition;
        }

        void Update()
        {
            bool isDrag = false;
            Vector3 inputPosition = Vector3.zero;
#if !UNITY_EDITOR
            if (Input.touches.Length > 0)
            {
                inputPosition = Input.GetTouch(0).position;

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        isStartOverUI = true;

                    startTouchPosition = inputPosition;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isStartOverUI = false;
                    currentPosition = root.localPosition;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved && !isStartOverUI)
                    isDrag = true;
            }
#else
            inputPosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    isStartOverUI = true;

                startTouchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isStartOverUI = false;
                currentPosition = root.localPosition;
            }

            if (Input.GetMouseButton(0) && !isStartOverUI)
                isDrag = true;
#endif

            if (isDrag) 
            { 
                Vector3 delta = startTouchPosition - inputPosition;
                currentPosition += new Vector3(delta.x, 0f, delta.y) / 50f;
                startTouchPosition = inputPosition;

                currentPosition = new Vector3(
                    Math.Clamp(currentPosition.x, minX, maxX),
                    currentPosition.y,
                    Math.Clamp(currentPosition.z, minZ, maxZ));

                root.localPosition = Vector3.Lerp(root.position, currentPosition, Time.unscaledTime * 10f);
            }
        }
    }
}
