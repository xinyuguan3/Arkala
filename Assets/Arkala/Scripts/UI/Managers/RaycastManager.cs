using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClickNext.Scripts.UI.Managers
{
    public class RaycastManager : MonoBehaviour
    {
        public static Camera CurrentCamera { get; private set; }

        public event Action<GameObject, Vector3> OnHit = delegate { };
        public event Action<GameObject, Vector3> OnBegan = delegate { };
        public event Action<GameObject, Vector3> OnDrag = delegate { };
        public event Action OnEnd = delegate { };

        [SerializeField] private Camera mainCamera;
        [SerializeField] private float defineDragDistance;

        private Vector3 startPosition;
        private int currentMask = Physics.AllLayers;

        private void Start() => CurrentCamera = mainCamera;

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                CheckRaycast(Input.mousePosition, OnBegan);
            }

            if (Input.GetMouseButtonUp(0))
            {
                var distance = Vector3.Distance(startPosition, Input.mousePosition);
                if(distance < defineDragDistance)
                    CheckRaycast(Input.mousePosition, OnHit);

                OnEnd();
            }

            if(Input.GetMouseButton(0))
                CheckRaycast(Input.mousePosition, OnDrag);
#else
            if (Input.touchCount > 0)
            {
               var touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    startPosition = touch.position;
                    CheckRaycast(Input.mousePosition, OnBegan);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    var distance = Vector3.Distance(startPosition, touch.position);
                    if (distance < defineDragDistance)
                        CheckRaycast(touch.position, OnHit);
            
                    OnEnd();
                }

                if (touch.phase == TouchPhase.Moved)
                    CheckRaycast(touch.position, OnDrag);
            }
#endif
        }

        private void CheckRaycast(Vector3 position, Action<GameObject, Vector3> OnRaycast)
        {
            Ray ray = CurrentCamera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask: currentMask))
                OnRaycast(hit.collider.gameObject, hit.point);
        }

        public void SetLayerMask(int layer) => currentMask = layer;

        public void SetDefaultCamera() => SetRaycastCamera(mainCamera);

        public void SetRaycastCamera(Camera camera)
        {
            CurrentCamera.gameObject.SetActive(false);
            CurrentCamera = camera;
            CurrentCamera.gameObject.SetActive(true);
        }
    }
}
