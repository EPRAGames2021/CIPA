using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EPRA.Utilities;

namespace CIPA
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PatrolSystem))]
    public class DodgeableTruck : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClipCollection _engineTurningOn;

        [SerializeField] private bool _turnedOn;
        [SerializeField] private bool _moving;
        [SerializeField, Min(0.1f)] private float _speed;

        [SerializeField] private PatrolSystem _patrolSystem;

        [SerializeField] private List<BumpableObject> _contactPoints;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetTurnOn(_turnedOn);
        }
#endif

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

            SetTurnOn(_turnedOn);
        }

        private void HandleAnimation()
        {
            _animator.SetBool("IsIdle", _turnedOn && !_moving);
            _animator.SetBool("IsMoving", _turnedOn && _moving);
        }


        public void InitiateTruckMovement()
        {
            if (!_turnedOn)
            {
                //_turnedOn = true;
                SetTurnOn(true);
            }

            _moving = true;

            float distance = Vector3.Distance(transform.position, _patrolSystem.CurrentTarget.position);
            float time = distance / _speed;

            transform.DOMove(_patrolSystem.CurrentTarget.position, time);

            AudioManager.Instance.PlayRandomSFX(_engineTurningOn, _audioSource);
        }

        public void SetTurnOn(bool turnOn)
        {
            _turnedOn = turnOn;

            for (int i = 0; i < _contactPoints.Count; i++)
            {
                _contactPoints[i].SetActive(_turnedOn);
            }
        }
    }
}
