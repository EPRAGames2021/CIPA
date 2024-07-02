using UnityEngine;
using EPRA.Utilities;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Job", menuName = "Scriptable Objects/Job", order = 1)]
public class JobSO : ScriptableObject
{
    [SerializeField] private JobSector _jobSector;

    [SerializeField] private int _score;
    [SerializeField] private string _jobName;
    [SerializeField] private string _saveName;

    [SerializeField] private string _keyName;

    [SerializeField] private List<EquipmentSO> _requiredEquipmentSOList;

    [SerializeField] private List<TrackableAction> _actions;

    public int Score => _score;
    public string JobName => _jobName;
    public string KeyName => _keyName;
    public List<EquipmentSO> RequiredEquipmentSO => _requiredEquipmentSOList;


    private void OnValidate()
    {
        _saveName = _jobSector.ToString() + _jobName;
    }

    private void OnEnable()
    {
        LoadData();
    }


    public void SetScore(int score)
    {
        _score = score;

        SaveData();
    }

    public void AddUniqueAction(string action, bool performed)
    {
        if (_actions.Count > 0)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i].Action == action)
                {
                    Debug.Log("Action has already been added. Updating value.");

                    _actions[i].Performed = performed;

                    return;
                }
            }
        }

        AddNewAction(action, performed);
    }

    private void AddNewAction(string action, bool performed)
    {
        TrackableAction trackableAction = new(action, performed);

        _actions.Add(trackableAction);

        SaveData();
    }

    private void LoadData()
    {
        _score = DataManager.HasData(_saveName) ? DataManager.LoadData<int>(_saveName) : 0;
        _actions = DataManager.HasData(_saveName + "_actions") ? DataManager.LoadData<List<TrackableAction>>(_saveName + "_actions") : new();
    }

    private void SaveData()
    {
        DataManager.SaveData<int>(_saveName, _score);
        DataManager.SaveData<List<TrackableAction>>(_saveName + "_actions", _actions);
    }
}
