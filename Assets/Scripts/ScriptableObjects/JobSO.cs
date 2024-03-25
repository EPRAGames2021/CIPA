using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Job", menuName = "Scriptable Objects/Job", order = 1)]
public class JobSO : ScriptableObject
{
    [SerializeField] private JobSector _jobSector;

    [SerializeField] private int _score;
    [SerializeField] private string _jobName;
    [SerializeField] private string _saveName;

    [SerializeField] private string _keyName;

    public int Score => _score;
    public string JobName => _jobName;
    public string KeyName => _keyName;


    private void OnValidate()
    {
        _saveName = _jobSector.ToString() + _jobName;
    }

    private void OnEnable()
    {
        LoadValue();
    }


    public void SetScore(int score)
    {
        _score = score;

        SaveValue();
    }

    private void LoadValue()
    {
        _score = DataManager.HasData(_saveName) ? DataManager.LoadData<int>(_saveName) : 0;
    }

    private void SaveValue()
    {
        DataManager.SaveData<int>(_saveName, _score);
    }
}
