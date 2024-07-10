using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class SupervisorTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;
        [SerializeField] private Supervisor _supervisor;


        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            _playerDetector.OnPlayerDetected += ReactToPlayer;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= ReactToPlayer;
        }


        private void ReactToPlayer(Player player)
        {
            _supervisor.Wave();

            _playerDetector.OnPlayerDetected -= ReactToPlayer;
        }
    }
}
