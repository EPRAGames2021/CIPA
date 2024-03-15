using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    [SerializeField] private JobSectorAreaSO _jobSectorAreaSO;

    public JobSectorAreaSO JobSectorAreaSO { get { return _jobSectorAreaSO; } set { _jobSectorAreaSO = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthSystem>() != null)
        {
            //temporary until the mini games themselves are created

            _jobSectorAreaSO.FinishDay();

            CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);

            GameManager.Instance.UpdateGameState(GameState.PausedState);
        }
    }
}
