using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Employee", menuName = "Scriptable Objects/Employee", order = 1)]
public class EmployeeSO : ScriptableObject
{
    [SerializeField] private string _lastID;
    [SerializeField] private List<JobSectorAreaSO> _sectors;

    [SerializeField] private List<JobAreaReport> _jobAreaReports;

    public List<JobAreaReport> JobAreaReports { get { return _jobAreaReports; } set { _jobAreaReports = value; } }


    public void OnEnable()
    {
        if (_jobAreaReports == null)
        {
            _jobAreaReports = new();
        }
    }



    private void SetID(string id)
    {
        _lastID = id;

        DataManager.SaveData<string>("LastID", _lastID);
    }

    public void SetupID(string id)
    {
        if (_lastID != FirebaseHandler.Instance.LoggedID)
        {
            _jobAreaReports = new();

            foreach (JobSectorAreaSO sector in _sectors)
            {
                sector.ResetDays();
            }
        }

        SetID(id);
    }

    public void AddNewDayReport(JobSector jobSector, DayReport dayReport)
    {
        bool hasFound = false;

        for (int i = 0; i < _jobAreaReports.Count; i++)
        {
            if (_jobAreaReports[i].JobSector == jobSector)
            {
                _jobAreaReports[i].AddNewDayReport(dayReport);

                hasFound = true;

                break;
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
        int day = dayReport.Day;

        if (day >= _dayReportList.Count)
        {
            _dayReportList.Add(dayReport);
        }
        else
        {
            for (int i = 0; i < day; i++)
            {
                if (day == _dayReportList[i].Day)
                {
                    _dayReportList.RemoveAt(day);
                    _dayReportList.Insert(day, dayReport);
                }
            }
        }

        _dayReportList.Sort((day1, day2) => day1.Day.CompareTo(day2.Day));
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

    public DayReport(int day)
    {
        _day = day;
        _score = 0;
        _actions = null;
    }
}
 