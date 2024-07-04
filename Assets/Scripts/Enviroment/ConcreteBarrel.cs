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
    [SerializeField] private float _idleTime;
    [SerializeField] private float _idleTimeTolarance;

    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClipCollection _spinningSFX;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        DetermineTouch();

        Spin();
        PlaySound();
    }


    private void Init()
    {
        _spinning = false;
    }


    private void DetermineTouch()
    {
        if (_screenTouchController.DetectHolding())
        {
            _spinning = true;

            _idleTime = 0f;
        }
        else
        {
            _idleTime += Time.deltaTime;
        }

        if (_idleTime >= _idleTimeTolarance)
        {
            _spinning = false;
        }
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

        if (_spinning && !_audioSource.isPlaying)
        {
            AudioManager.Instance.PlayRandomSFX(_spinningSFX, _audioSource);
        }
        else if (!_spinning && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
