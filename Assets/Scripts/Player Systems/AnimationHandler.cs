using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClipCollection _stepSFX;
        [SerializeField] private GameObject _collisionVFXPrefab;

        [SerializeField] private Animator _animator;
        [SerializeField] private Player _player;
        [SerializeField] private MovementSystem _movementSystem;

        private void OnValidate()
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        }

        private void Awake()
        {
            if (_animator != null) _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            Animate();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            if (_player != null) _player.OnStateChange += AdaptToState;

            //this must not be here but I'm too lazy to fix this properly
            if (_animator != null) _animator.updateMode = AnimatorUpdateMode.Normal;
        }

        private void Finish()
        {
            if (_player != null) _player.OnStateChange -= AdaptToState;
        }


        private void AdaptToState(CharacterState characterState)
        {
            if (characterState == CharacterState.Dying)
            {
                _animator.SetTrigger("Die");
            }
            else if (characterState == CharacterState.Dancing)
            {
                _animator.SetTrigger("Win");
            }

            _animator.SetBool("IsLooking", characterState == CharacterState.Looking);
        }

        private void Animate()
        {
            if (_movementSystem == null) return;

            _animator.SetBool("IsWalking", _movementSystem.IsWalking && _player.State.Equals(CharacterState.Roaming));
            _animator.SetBool("IsRunning", _movementSystem.IsRunning && _player.State.Equals(CharacterState.Roaming));
        }

        public void CallSFX()
        {
            AudioManager.Instance.PlayRandomSFX(_stepSFX, _audioSource);
        }

        public void CallCollisionVFX()
        {
            GameObject virtualEntity = InputHandler.Instance.CurrentMovementSystem.gameObject;

            GameObject vfx = Instantiate(_collisionVFXPrefab, virtualEntity.transform.position, virtualEntity.transform.rotation, virtualEntity.transform.parent);

            Destroy(vfx, 1f);
        }
    }
}
