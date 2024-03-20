using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(fileName = "Score List", menuName = "Scriptable Objects/Score List", order = 1)]
public class DayScoreListSO : ScriptableObject
{
    [SerializeField] private List<int> _scores;
    [SerializeField] private string _saveName;

    public List<int> Scores => _scores;


    private void OnEnable()
    {
        LoadValue();
    }


    public void SetScoreToDay(int day, int score)
    {
        _scores.Insert(day, score);

        SaveValue();
    }

    private void LoadValue()
    {
        _scores = DataManager.HasData(_saveName) ? DataManager.LoadData<List<int>>(_saveName) : new();
    }

    private void SaveValue()
    {
        DataManager.SaveData<List<int>>(_saveName, _scores);
    }
}
