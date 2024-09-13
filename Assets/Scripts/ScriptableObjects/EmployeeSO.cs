using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;
using System.Linq;

[CreateAssetMenu(fileName = "Employee", menuName = "Scriptable Objects/Employee", order = 1)]
public class EmployeeSO : ScriptableObject
{
    [SerializeField] private List<JobAreaReport> _jobAreaReports;

    public void AddNewDayReport(JobSector jobSector, DayReport dayReport)
    {
        bool hasFound = false;

        for (int i = 0; i < _jobAreaReports.Count; i++)
        {
            if (_jobAreaReports[i].JobSector == jobSector)
            {
                _jobAreaReports[i].AddNewDayReport(dayReport);

                hasFound = true;
            }
        }

        if (!hasFound)
        {
            JobAreaReport jobAreaReport = new(jobSector);

            _jobAreaReports.Add(jobAreaReport);

            jobAreaReport.AddNewDayReport(dayReport);
        }
    }
}

[System.Serializable]
public class JobAreaReport
{
    [SerializeField] private JobSector _jobSector;
    [SerializeField] private List<DayReport> _dayReportList;

    public JobSector JobSector => _jobSector;
    public List<DayReport> DayReportList => _dayReportList;


    public JobAreaReport(JobSector jobSector)
    {
        _jobSector = jobSector;
        _dayReportList = new List<DayReport>();
    }

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
 