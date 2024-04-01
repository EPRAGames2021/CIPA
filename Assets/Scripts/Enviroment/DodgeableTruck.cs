using UnityEngine;
using EPRA.Utilities;

public class DodgeableTruck : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;

    [SerializeField] private Animator _animator;


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
        _playerDetector.OnPlayerDetected += InitiateTruckMovement;
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= InitiateTruckMovement;
    }


    private void InitiateTruckMovement()
    {
        //Debug.Log("Truck movement started");

        _animator.SetTrigger("Reverse");
    }
}
