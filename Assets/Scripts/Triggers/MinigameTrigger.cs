using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    [SerializeField] private JobSectorAreaSO _jobSectorAreaSO;

    [SerializeField] private PlayerDetector _playerDetector;

    [SerializeField] private Player _player;

    public JobSectorAreaSO JobSectorAreaSO { get { return _jobSectorAreaSO; } set { _jobSectorAreaSO = value; } }


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
        _playerDetector.OnPlayerDetected += InitiateMinigame;
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= InitiateMinigame;
    }


    private void InitiateMinigame()
    {
        //temporary until the mini games themselves are created

        _jobSectorAreaSO.FinishDay();

        CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);

        GameManager.Instance.UpdateGameState(GameState.PausedState);
    }
}
