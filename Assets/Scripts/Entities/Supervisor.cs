using UnityEngine;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class Supervisor : MonoBehaviour
    {
        [Header("GD area")]
        [SerializeField] private float _distanceToTriggerSpeech;

        [Header("Dev area")]
        [SerializeField] private Player _player;
        [SerializeField] private Animator _animator;

        [SerializeField] private TextMeshProUGUI _missionLineReminder;
        [SerializeField] private GameObject _speechBubble;
        [SerializeField] private GameObject _exclamationMark;
        [SerializeField] private GameObject _canvas;

        [Header("Debug")]
        [SerializeField] private float _distance;
        [SerializeField] private bool _displaying;


        private void Start()
        {
            DisplayMessage(false);
        }

        private void Update()
        {
            CheckDistance();
        }

        private void LateUpdate()
        {
            _canvas.transform.LookAt(Camera.main.transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _distanceToTriggerSpeech);
        }


        private void CheckDistance()
        {
            _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance <= _distanceToTriggerSpeech && !_displaying)
            {
                DisplayMessage(true);
            }
            else if (_distance > _distanceToTriggerSpeech && _displaying)
            {
                DisplayMessage(false);
            }
        }

        private void DisplayMessage(bool display)
        {
            _displaying = display;

            _exclamationMark.SetActive(!_displaying);
            _speechBubble.SetActive(_displaying);

            if (_player.EquipmentSystem.WearingEquipment)
            {
                JobSector jobSector = JobAreaManager.Instance.JobSectorAreaSO.JobSector;
                int day = JobAreaManager.Instance.JobSectorAreaSO.Day;

                string key = jobSector + "Day" + day + "supervisor";

                _missionLineReminder.text = LanguageManager.GetTranslation(key);
            }
            else
            {
                _missionLineReminder.text = LanguageManager.GetTranslation("supervisorEquipmentReminder");
            }
        }

        public void Wave()
        {
            _animator.SetTrigger("WavingTrigger");
        }
    }
}
