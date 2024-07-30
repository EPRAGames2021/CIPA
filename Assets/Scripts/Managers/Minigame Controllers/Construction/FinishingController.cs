using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class FinishingController : BaseController
    {
        [Header("GD area")]
        [SerializeField] private float _paintMaxTime;
        [SerializeField] private float _paintMaxTimeWrongSpeed;
        [SerializeField] private float _idealSpeed;
        [Range(0, 100), SerializeField] private float _speedTolerancePercent;
        [SerializeField] private float _paintMinTolerableSpeed;
        [SerializeField] private float _paintMaxTolerableSpeed;

        [Header("Art")]
        [SerializeField] private ParticleSystem _poofVFX;
        [SerializeField] private ParticleSystem _sparklesVFX;

        [Header("Dev")]
        [SerializeField] private Transform _playerTeleportDestination;

        [SerializeField] private FinishingUI _finishingUI;
        [SerializeField] private PaintingRoom _paintingRoom;
        [SerializeField] private List<InkRollHandler> _inkRollHandlers;
        [SerializeField] private int _logsSize;

        private InkRollHandler _currentRoll;
        private int _stageIndex;
        private float _averageSpeed;
        private List<float> _speedLogs = new();
        private float _paintTimeWrongSpeed;
        private float _paintTime;
        private bool _paintFinished;

        private ScreenTouchController _screenTouchController;
        private MouseDelta _mouseDelta;
        private MousePositionHandler _mousePositionHandler;


        public float CompletionPercentage => (_paintTime / _paintMaxTime) * 100;


        private void OnValidate()
        {
            if (_finishingUI != null) _finishingUI.PaintSlider.maxValue = _idealSpeed * 2;

            _paintMaxTolerableSpeed = _idealSpeed + ((_idealSpeed / 100) * _speedTolerancePercent);
            _paintMinTolerableSpeed = _idealSpeed - ((_idealSpeed / 100) * _speedTolerancePercent);
        }

        private void Update()
        {
            if (_paintFinished) return;
            if (!_screenTouchController.FirstPress) return;

            if (_screenTouchController.DetectHolding())
            {
                CalculateAverageSpeed();
                DeterminePaintingTime();

                _currentRoll.UpdateRollPosition(_mousePositionHandler.HeightPercent);
                _finishingUI.PaintSlider.value = _averageSpeed;
            }

            _currentRoll.SetCompletion(CompletionPercentage);
            _currentRoll.SetPainting(_screenTouchController.DetectHolding());

            CheckForMiniGameCompletion();
        }

        protected override void Init()
        {
            _screenTouchController = _finishingUI.ScreenTouchController;
            _mouseDelta = _finishingUI.MouseDelta;
            _mousePositionHandler = _finishingUI.MousePositionHandler;

            _poofVFX.gameObject.SetActive(false);
            _sparklesVFX.gameObject.SetActive(false);

            base.Init();
        }

        protected override void StartMiniGame()
        {
            base.StartMiniGame();

            ResetSpeedAndTimers();

            for (int i = 0; i < _inkRollHandlers.Count; i++)
            {
                _inkRollHandlers[i].Refresh();

                _inkRollHandlers[i].gameObject.SetActive(false);
            }

            SetStage(0);
        }

        private void ResetSpeedAndTimers()
        {
            _averageSpeed = 0;

            _speedLogs.Clear();
            for (int i = 0; i < _logsSize; i++)
            {
                _speedLogs.Add(_idealSpeed);
            }

            _paintFinished = false;
            _paintTimeWrongSpeed = 0.0f;
            _paintTime = 0.0f;

            _finishingUI.PaintSlider.minValue = 0;
            _finishingUI.PaintSlider.value = _idealSpeed;
            _finishingUI.PaintSlider.maxValue = _idealSpeed * 2;
        }

        private void CalculateAverageSpeed()
        {
            _speedLogs.Add(_mouseDelta.Speed / 1000);

            if (_speedLogs.Count > _logsSize)
            {
                _speedLogs.RemoveAt(0);
            }

            float total = 0;

            foreach (float speed in _speedLogs)
            {
                total += speed;
            }

            _averageSpeed = total / _speedLogs.Count;
        }

        private void DeterminePaintingTime()
        {
            bool tolerableSpeed = _averageSpeed > _paintMinTolerableSpeed && _averageSpeed < _paintMaxTolerableSpeed;

            if (tolerableSpeed)
            {
                _paintTime += Time.deltaTime;
            }
            else if (!tolerableSpeed && _mouseDelta.Distance > 0)
            {
                _paintTimeWrongSpeed += Time.deltaTime;
            }
        }

        private void CheckForMiniGameCompletion()
        {
            if (_paintFinished) return;

            if (_paintTime >= _paintMaxTime)
            {
                _currentRoll.SetPainting(false);
                _currentRoll.SetCompleted(true);

                if (_stageIndex < _inkRollHandlers.Count - 1)
                {
                    NextStage();
                    ResetSpeedAndTimers();
                }
                else
                {
                    JobAreaManager.Instance.FinishMinigame(true);
                    StartCoroutine(TeleportPlayerDelay());
                    
                    _paintFinished = true;
                }
            }
            else if (_paintTimeWrongSpeed >= _paintMaxTimeWrongSpeed)
            {
                JobAreaManager.Instance.FinishMinigame(false);

                _paintFinished = true;
            }
        }

        private void SetStage(int stageIndex)
        {
            _stageIndex = stageIndex;

            _currentRoll = _inkRollHandlers[_stageIndex];

            _currentRoll.gameObject.SetActive(true);
        }

        private void NextStage()
        {
            _stageIndex++;

            SetStage(_stageIndex);
        }

        private IEnumerator TeleportPlayerDelay()
        {
            TeleportPlayer();

            yield return new WaitForSeconds(1f);

            _paintingRoom.TransformRoom();
        }

        private void TeleportPlayer()
        {
            _player.MovementSystem.CanMove = false;
            _player.transform.SetLocalPositionAndRotation(_playerTeleportDestination.position, _playerTeleportDestination.rotation);
            StartCoroutine(AnimationDelay());
        }

        private IEnumerator AnimationDelay()
        {
            yield return new WaitForSeconds(2);

            _player.ChangeState(CharacterState.Looking);

            _sparklesVFX.gameObject.SetActive(true);
            _sparklesVFX.Play();

            _poofVFX.gameObject.SetActive(true);
            _poofVFX.Play();
        }
    }
}
