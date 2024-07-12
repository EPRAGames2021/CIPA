using System.Collections;
using UnityEngine;

namespace CIPA
{
    public class FadeScreen : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private float _delay;


        public void InitiateFadeSequence()
        {
            StartCoroutine(FadeSequenceDelay());
        }

        private IEnumerator FadeSequenceDelay()
        {
            FadeIn();

            FadeOut();

            yield return new WaitForSeconds(_delay);

            _animator.SetBool("FadeIn", false);
            _animator.SetBool("FadeOut", false);
        }

        private void FadeIn()
        {
            _animator.SetBool("FadeIn", true);
        }

        private void FadeOut()
        {
            _animator.SetBool("FadeOut", true);
        }

    }
}