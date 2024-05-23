using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAndPenaltyManager : MonoBehaviour
{
    public static RewardAndPenaltyManager Instance;

    [SerializeField] private CurrencySO _dayScore;

    [Header("Rewards")]
    [SerializeField] private RewardsAndPenaltiesSO _equipEquipment;
    [SerializeField] private RewardsAndPenaltiesSO _arriveAtJob;
    [SerializeField] private RewardsAndPenaltiesSO _completeJob;

    [Header("Penalties")]
    [SerializeField] private RewardsAndPenaltiesSO _playerBumpedIntoObject;
    [SerializeField] private RewardsAndPenaltiesSO _failJob;
    [SerializeField] private RewardsAndPenaltiesSO _arriveAtJobUnequipped;

    private void Awake()
    {
        InitSingleton();
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

    public void ResetScore()
    {
        _dayScore.SetCurrencyValue(0);
    }

    public void PlayerHasEquippedEquipment()
    {
        ChangeScore(_equipEquipment);
    }

    public void PlayerHasArrivedAtJob()
    {
        ChangeScore(_arriveAtJob);
    }

    public void PlayerHasCompletedJob()
    {
        ChangeScore(_completeJob);
    }

    public void PlayerHasBumpedIntoObject()
    {
        ChangeScore(_playerBumpedIntoObject);
    }

    public void PlayerHasFailedJob()
    {
        ChangeScore(_failJob);
    }

    public void PlayerHasArrivedAtJobUnequipped()
    {
        ChangeScore(_arriveAtJobUnequipped);
    }


    private void ChangeScore(RewardsAndPenaltiesSO rewardsAndPenaltiesSO)
    {
        if (rewardsAndPenaltiesSO.IsPenalty)
        {
            _dayScore.RemoveFromCurrency(rewardsAndPenaltiesSO.Score);
        }
        else
        {
            _dayScore.AddToCurrency(rewardsAndPenaltiesSO.Score);
        }
    }
}
