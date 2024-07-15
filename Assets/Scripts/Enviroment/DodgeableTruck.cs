using UnityEngine;
using DG.Tweening;
using EPRA.Utilities;

namespace CIPA
{
    [RequireComponent(typeof(PatrolSystem))]
    [RequireComponent(typeof(AudioSource))]
    public class DodgeableTruck : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClipCollection _engineTurningOn;

        [SerializeField] private bool _turnedOn;
        [SerializeField] private bool _moving;
        [SerializeField, Min(0.1f)] private float _speed;

        [SerializeField] private PatrolSystem _patrolSystem;


        private void Start()
        {
            Init();
        }

        private void Update()
        {
            HandleAnimation();
        }


        private void Init()
        {
            if (_patrolSystem.HasTargets) transform.LookAt(_patrolSystem.CurrentTarget);
        }

        private void HandleAnimation()
        {
            _animator.SetBool("IsIdle", _turnedOn && !_moving);
            _animator.SetBool("IsMoving", _turnedOn && _moving);
        }


        public void InitiateTruckMovement()
        {
            if (!_turnedOn) _turnedOn = true;

            _moving = true;

            float distance = Vector3.Distance(transform.position, _patrolSystem.CurrentTarget.position);
            float time = distance / _speed;

            transform.DOMove(_patrolSystem.CurrentTarget.position, time);

            AudioManager.Instance.PlayRandomSFX(_engineTurningOn, _audioSource);
        }
    }
}
