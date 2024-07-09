using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance;

        [SerializeField] private int _currentMissionIndex;

        public int CurrentMissionIndex => _currentMissionIndex;

        public static event System.Action<int> OnMissionChanged;


        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            Init();
        }


        private void InitSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Init()
        {
            _currentMissionIndex = 0;

            OnMissionChanged?.Invoke(_currentMissionIndex);
        }

        public void GoToNextMission()
        {
            _currentMissionIndex++;

            OnMissionChanged?.Invoke(_currentMissionIndex);
        }

        public void ReturnToPreviousMission()
        {
            _currentMissionIndex--;

            OnMissionChanged?.Invoke(_currentMissionIndex);
        }

        public void GoToMission(int missions)
        {
            _currentMissionIndex = missions;

            OnMissionChanged?.Invoke(_currentMissionIndex);
        }
    }
}
