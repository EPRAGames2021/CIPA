using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Employee", menuName = "Scriptable Objects/Employee", order = 1)]
public class EmployeeSO : ScriptableObject
{
    [SerializeField] private List<DayReport> _dayReportList;

    public void AddNewDayReport(DayReport dayReport)
    {
        int index = dayReport.Day;

        if (index < _dayReportList.Count)
        {
            _dayReportList.RemoveAt(index);
        }

        _dayReportList.Insert(dayReport.Day, dayReport);
    }
}


[System.Serializable]
public class DayReport
{
    [SerializeField] private int _day;
    [SerializeField] private int _score;
    [SerializeField] private List<TrackableAction> _actions;

    public int Day => _day;
    public int Score => _score;
    public List<TrackableAction> Actions => _actions;

    public DayReport(int day, int score, List<TrackableAction> actions)
    {
        _day = day;
        _score = score;
        _actions = actions;
    }
}
