using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class GridGameController : BaseController
    {
        [SerializeField] private PipeGrid _grid;

        protected override void StartMiniGame()
        {
            base.StartMiniGame();

            Camera.main.orthographic = true;

            _grid.ResetGrid();
        }

        protected override void EndMinigame()
        {
            base.EndMinigame();

            Camera.main.orthographic = false;

            _grid.LockGrid();
        }

        public bool CheckIfGridIsCorrect()
        {
            return _grid.CheckForCorrectGrid();
        }
    }
}
