using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Supervisor : MonoBehaviour
{
    [Header("GD area")]
    [SerializeField] private float _distanceToTriggerSpeech;

    [Header("Dev area")]
    [SerializeField] private Player _player;

    [SerializeField] private GameObject _speechBubble;
    [SerializeField] private TextMeshProUGUI _missionLineReminder;
    [SerializeField] private GameObject _exclamationMark;

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
        _speechBubble.transform.LookAt(Camera.main.transform.position);
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
        _missionLineReminder.gameObject.SetActive(_displaying);

        if (_player.EquipmentSystem.WearingEquipment)
        {
            int day = JobAreaManager.Instance.JobSectorAreaSO.Day;
            string key = "day" + day + "supervisor";

            _missionLineReminder.text = LanguageManager.GetTranslation(key);
        }
        else
        {
            _missionLineReminder.text = LanguageManager.GetTranslation("supervisorEquipmentReminder");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distanceToTriggerSpeech);
    }

}
