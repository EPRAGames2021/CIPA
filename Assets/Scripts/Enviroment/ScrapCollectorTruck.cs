using UnityEngine;
using DG.Tweening;
using ES3Types;
using System.Collections;

namespace CIPA
{
    public class ScrapCollectorTruck : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField] private Transform _dockedPosition;
        [SerializeField] private Transform _awayPosition;

        [SerializeField] private float _stationaryTime;

        [SerializeField] private float _speed;
        [SerializeField] private float _leavingSpeed;
        [SerializeField] private float _returningSpeed;

        [Header("Debug")]
        [SerializeField] private float _distanceBetweenPoints;

        private Coroutine _stationaryCoroutine;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            _distanceBetweenPoints = Vector3.Distance(_dockedPosition.transform.position, _awayPosition.transform.position);

            DrivingBehaviour();
        }


        private void Init()
        {
            _target = _awayPosition;
            _stationaryCoroutine = null;

            DrivingBehaviour();
        }

        private void Move()
        {
            transform.DOMove(_target.transform.position, _distanceBetweenPoints / _speed);
        }

        private void DrivingBehaviour()
        {
            if (_stationaryCoroutine == null)
            {
                if (_target == _dockedPosition)
                {
                    _target = _awayPosition;
                    _speed = _leavingSpeed;
                }
                else
                {
                    _target = _dockedPosition;
                    _speed = _returningSpeed;
                }

                _stationaryCoroutine = StartCoroutine(Drive());
            }
        }

        private IEnumerator Drive()
        {
            float travelingTime = _distanceBetweenPoints / _speed;

            yield return new WaitForSeconds(_stationaryTime + travelingTime + 1);

            Move();

            _stationaryCoroutine = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_dockedPosition.position, 1);
            Gizmos.DrawWireSphere(_awayPosition.position, 1);
        }
    }
}
