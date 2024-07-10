using UnityEngine;

namespace CIPA
{
    public class CarriableObject : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;


        private void Start()
        {
            Init();
        }


        private void Init()
        {
            _collider.enabled = false;

            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }


        public void Fall()
        {
            _collider.enabled = true;
            _collider.isTrigger = false;

            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;

            _rigidbody.AddRelativeForce(Vector3.forward * 10, ForceMode.Impulse);
        }

    }
}
