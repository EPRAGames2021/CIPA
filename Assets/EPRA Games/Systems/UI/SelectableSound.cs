using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EPRA.Utilities
{
    public class SelectableSound : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler
    {
        [SerializeField] private Selectable _selectable;

        [SerializeField] private AudioClipCollection _onSelectSound;
        [SerializeField] private AudioClipCollection _onSubmitSound;

        [SerializeField] private bool _randomFX;

        [Min(0)]
        [SerializeField] private int _FXIndex;

        private void OnValidate()
        {
            if (_selectable == null)
            {
                Debug.LogWarning(this + " has no selectable associated with it.");
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            HandleSelection();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            HandleSubmission();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HandleSubmission();
        }

        private void HandleSelection()
        {
            if (_onSelectSound == null) return;
            if (!gameObject.activeInHierarchy) return;

            if (_randomFX)
            {
                AudioManager.Instance.PlayRandomSFX(_onSelectSound);
            }
            else
            {
                AudioManager.Instance.PlaySFX(_onSelectSound, _FXIndex);
            }            
        }

        private void HandleSubmission()
        {
            if (_onSubmitSound == null) return;
            if (!gameObject.activeInHierarchy) return;

            if (_randomFX)
            {
                AudioManager.Instance.PlayRandomSFX(_onSubmitSound);
            }
            else
            {
                AudioManager.Instance.PlaySFX(_onSubmitSound, _FXIndex);
            }
        }

        public void PlaySelection()
        {
            HandleSelection();
        }

        public void PlaySubmission()
        {
            HandleSubmission();
        }
    }
}
