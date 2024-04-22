using UnityEngine;
using DG.Tweening;
using EPRA.Utilities;

[RequireComponent(typeof(AudioSource))]
public class ConcreteBarrel : MonoBehaviour
{
    [SerializeField] private ScreenTouchController _screenTouchController;
    [SerializeField] private ConcreteMixPanel _concreteMixPanel;
    [SerializeField] private GameObject _concreteBarrel;

    [SerializeField] private bool _spinning;

    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClipCollection _spinningSFX;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _spinning = _screenTouchController.DetectHolding();

        Spin();
        PlaySound();
    }


    private void Init()
    {
        _spinning = false;
    }

    private void Spin()
    {
        if (_concreteMixPanel.MixFinished) return;

        if (_spinning)
        {
            _concreteBarrel.transform.Rotate(0, 0, _concreteMixPanel.AverageSpeed * 1.5f);
            //_concreteBarrel.transform.DORotate(new Vector3(0, 0, _concreteMixPanel.AverageSpeed * 1.5f), 1f);
        }
    }


    private void PlaySound()
    {
        if (_concreteMixPanel.MixFinished)
        {
            _audioSource.Stop();

            return;
        }

        if (_spinning && !_audioSource.isPlaying) AudioManager.Instance.PlayRandomSFX(_audioSource, _spinningSFX);
        if (!_spinning && _audioSource.isPlaying) _audioSource.Stop();
    }
}
