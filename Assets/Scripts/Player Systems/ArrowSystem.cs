using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CIPA
{
    public class ArrowSystem : MonoBehaviour
    {
        [SerializeField] private GameObject _arrow;

        [Header("Dev area")]
        [SerializeField] private Transform _target;
        [SerializeField] private int _index;
        [SerializeField] private List<Transform> _targets;

        [Header("Debug")]
        [SerializeField] private float _distance;
        [SerializeField] private float _toleranceDistance = 3f;


        private void Start()
        {
            Init();
        }

        private void LateUpdate()
        {
            PointArrow();
            CheckDistance();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            _arrow.SetActive(false);

            ResetArrow();

            CheckArrow(MissionManager.Instance.CurrentMissionIndex);
            MissionManager.OnMissionChanged += CheckArrow;
        }

        private void Finish()
        {
            MissionManager.OnMissionChanged -= CheckArrow;
        }


        private void ResetArrow()
        {
            _index = 0;

            if (_targets.Count > 0)
            {
                _target = _targets[_index];
            }
            else
            {
                Debug.LogWarning("There are no targets.");
            }
        }

        private void PointArrow()
        {
            if (_target == null) return;

            _arrow.transform.DOLookAt(_target.position, 0.5f);
        }

        private void CheckDistance()
        {
            if (_target == null) return;

            _distance = Vector3.Distance(transform.position, _targets[_index].position);

            if (_distance < _toleranceDistance && _index < _targets.Count - 1)
            {
                _index++;
                _target = _targets[_index];
            }
        }

        private void CheckArrow(int missionIndex)
        {
            int day = JobAreaManager.Instance.JobSectorAreaSO.Day;

            _arrow.SetActive(missionIndex > 0 && day < 2);
        }


        public void SetPath(List<Transform> newPath)
        {
            _targets.Clear();

            _targets = newPath;

            ResetArrow();
        }
    }
}
