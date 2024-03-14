using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [field: SerializeField] public GameState State { get; private set; }
        [field: SerializeField] public GameState PreviousState { get; private set; }

        public event System.Action<GameState> OnGameStateChanged;


        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            UpdateGameState(GameState.MainMenuState);
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


        public void UpdateGameState(GameState gameState)
        {
            if (State == gameState) return;

            State = gameState;

            OnGameStateChanged?.Invoke(gameState);
        }

        public void ReturnToPreviousState()
        {
            UpdateGameState(PreviousState);
        }
    }

    public enum GameState
    {
        MainMenuState = 0,
        GameState = 1,
        PausedState = 2,
    }
}
