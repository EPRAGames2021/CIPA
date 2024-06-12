using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    [CreateAssetMenu(fileName = "Job Sector Area", menuName = "Scriptable Objects/Job Sector Area", order = 1)]
    public class JobSectorAreaSO : ScriptableObject
    {
        [SerializeField] private JobSector _jobSectorAreaSO;

        [SerializeField] private List<JobSO> _jobs;

        [SerializeField] private int _day;

        public JobSector JobSector => _jobSectorAreaSO;
        public List<JobSO> Jobs => _jobs;
        public JobSO CurrentJob => _jobs[_day];
        public int Day => _day;
        public int TotalDays => _jobs.Count;
        public bool IsFinalDay => _day == TotalDays - 1;


        public event System.Action OnDayEnded;


        private void OnEnable()
        { 
            LoadData();
        }


        public void FinishDay()
        {
            if (!IsFinalDay)
            {
                _day++;
            }

            OnDayEnded?.Invoke();

            SaveData();

            //send list of day actions to data base here
        }

        public void SetScoreToDay(int day, int score)
        {
            _jobs[day].SetScore(score);

            SaveData();
        }


        private void LoadData()
        {
            _day = DataManager.HasData(_jobSectorAreaSO.ToString() + "_day") ? DataManager.LoadData<int>(_jobSectorAreaSO.ToString() + "_day") : 0;
        }

        private void SaveData()
        {
            DataManager.SaveData<int>(_jobSectorAreaSO.ToString() + "_day", _day);
        }
    }

    public enum JobSector
    {
        None = 0,
        Construction = 1,
        SteelIndustry = 2,
    }
}
