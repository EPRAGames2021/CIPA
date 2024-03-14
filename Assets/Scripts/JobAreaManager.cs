using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobAreaManager : MonoBehaviour
{
    [SerializeField] private JobSectorAreaSO _jobSectorSO;

    [SerializeField] private MinigameTrigger _miniGameTrigger;

    [SerializeField] private Enemy _player;


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
        _miniGameTrigger.JobSectorAreaSO = _jobSectorSO;

        GameManager.Instance.UpdateGameState(GameState.GameState);

        CanvasManager.Instance.GameScreen.SetDay(_jobSectorSO.Day);

        _player.HealthSystem.OnDied += PlayerDied;
    }

    private void Finish()
    {
        _player.HealthSystem.OnDied -= PlayerDied;
    }


    private void PlayerDied()
    {
        CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);
    }

}
