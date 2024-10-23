using System.Collections;
using UnityEngine;

namespace CIPA
{
    public class FadeScreen : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private float _delay;

        private Coroutine _coroutine;


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _coroutine = null;
        }


        public void InitiateFadeSequence()
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(FadeSequenceDelay());
            }
            else
            {
                Debug.Log("Fade sequence already running");
            }
        }

        private IEnumerator FadeSequenceDelay()
        {
            FadeIn(true);
            FadeOut(true);

            yield return new WaitForSeconds(_delay);

            FadeIn(false);
            FadeOut(false);

            _coroutine = null;
        }

        private void FadeIn(bool fade)
        {
            _animator.SetBool("FadeIn", fade);
        }

        private void FadeOut(bool fade)
        {
            _animator.SetBool("FadeOut", fade);
        }

    }
}