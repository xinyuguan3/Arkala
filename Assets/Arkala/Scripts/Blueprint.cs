using UnityEngine;

namespace ClickNext.Scripts
{
    public class Blueprint : MonoBehaviour
    {
        public Vector3 MenuLocation { get { return menuLocation? menuLocation.position: transform.position; } }

        [SerializeField] private GameObject structure;
        [SerializeField] private Transform menuLocation;
        [SerializeField] private Collider hitCollider;

        public void StartBuild(Transform root)
        {
            var building = Instantiate(structure, root);
                building.transform.position = transform.position;
                building.transform.rotation = transform.rotation;
        }

        public void HideCollider(bool value) =>
            hitCollider.enabled = !value;
    }
}
