using ClickNext.Scripts.Units.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace ClickNext.Scripts.Units
{
    public class Building : StateUnit
    {
        public Camera CameraView { get { return innerRoom? innerRoom.View: null; } }

        [SerializeField] private Household household;
        
        // private OffMeshLink offMeshLink;
        private Household innerRoom;

        protected new void Awake()
        {
            // offMeshLink = GetComponent<OffMeshLink>();
            // if (offMeshLink)
            //     offMeshLink.activated = false;

            base.Awake();
        }

        private void OnEnable() => OnStateChanged += StateChanged;
        private void OnDisable() => OnStateChanged -= StateChanged;

        private void StateChanged(int stateIndex)
        {
            //if the building is fully constructed, create the inner look of household.
            if (stateIndex == _maxState)
            {
                // if (household && offMeshLink)
                // {
                //     innerRoom = Instantiate(household);
                //     innerRoom.SetPosition();

                //     offMeshLink.endTransform = innerRoom.OffMeshLink;
                //     offMeshLink.activated = true;
                // }
            }
        }

        private void Start()
        {
            _targetTag = TagType.Building;
            Current.hp = 0f;
            UpdateState(0);
        }
    }
}