using UnityEngine;

namespace ClickNext.Scripts
{
    public class Household : MonoBehaviour
    {
        private static float SPAWN_LENGTH = 110f;
        public static float SPAWN_POSITION_Z;

        public Camera View { get { return view; } }
        [SerializeField] private Camera view;

        public Transform OffMeshLink { get { return offMeshLink; } }
        [SerializeField] private Transform offMeshLink;

        public void SetPosition()
        {
            SPAWN_POSITION_Z += SPAWN_LENGTH;
            transform.position = new Vector3(0f, 0f, SPAWN_POSITION_Z);
        }
    }
}
