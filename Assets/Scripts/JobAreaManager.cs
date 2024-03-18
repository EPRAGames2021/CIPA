using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobAreaManager : MonoBehaviour
{
    [SerializeField] private JobSectorAreaSO _jobSectorSO;

    [SerializeField] private MinigameTrigger _miniGameTrigger;

    [SerializeField] private Player _player;

    [SerializeField] private List<TrafficCone> _trafficConeList;

    [SerializeField] private CurrencySO _dayScore;

    [Header("Scores and penalties")]
    [SerializeField] private int _equipEquipmentScore;
    [SerializeField] private int _hitTrafficConePenalty;


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
        _dayScore.SetCurrencyValue(0);

        _miniGameTrigger.JobSectorAreaSO = _jobSectorSO;

        GameManager.Instance.UpdateGameState(GameState.GameState);
        CanvasManager.Instance.GameScreen.SetDay(_jobSectorSO.Day);

        _player.HealthSystem.OnDied += PlayerDied;
        _player.OnEquip += EquipPlayer;

        foreach (TrafficCone trafficCone in _trafficConeList)
        {
            trafficCone.OnDisplaced += PlayerHitTrafficCone;
        }
    }

    private void Finish()
    {
        _player.HealthSystem.OnDied -= PlayerDied;
        _player.OnEquip -= EquipPlayer;
    }


    private void PlayerDied()
    {
        CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);
    }

    private void EquipPlayer(bool equip)
    {
        if (equip)
        {
            _dayScore.AddToCurrency(_equipEquipmentScore);
        }
    }

    private void PlayerHitTrafficCone(TrafficCone trafficCone)
    {
        _dayScore.RemoveFromCurrency(_hitTrafficConePenalty);

        for (int i = 0; i < _trafficConeList.Count; i++)
        {
            if (_trafficConeList[i] == trafficCone)
            {
                trafficCone.OnDisplaced -= PlayerHitTrafficCone;
            }
        }
    }

}
