using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    [CreateAssetMenu(fileName = "Job Sector Area", menuName = "Scriptable Objects/Job Sector Area", order = 1)]
    public class JobSectorAreaSO : ScriptableObject
    {
        [SerializeField] private JobSector _jobSectorAreaSO;

        [SerializeField] private int _day;

        [Header("Because days start on 0, last day will always be Total Days - 1")]
        [SerializeField] private int _totalDays;

        public int Day => _day;
        public int TotalDays => _totalDays;
        public int FinalDay => _totalDays - 1;
        public bool IsFinalDay => _day == FinalDay;


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
