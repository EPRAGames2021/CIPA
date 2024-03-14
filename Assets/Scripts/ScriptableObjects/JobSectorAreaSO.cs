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

        [SerializeField] private int _totalDays;

        public int Day => _day;


        public event System.Action OnDayEnded;


        private void OnEnable()
        { 
            LoadData();
        }


        public void FinishDay()
        {
            _day++;

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
