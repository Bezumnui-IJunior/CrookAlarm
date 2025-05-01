using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CrookDetector))]
[RequireComponent(typeof(Collider))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _reachMaxSeconds;
    [SerializeField] private float _maxVolume = 1;

    private CrookDetector _crookDetector;
    private AudioSource _source;
    private WaitForFixedUpdate _updateDelay;

    private bool _isWork;
    private bool _isPlaySound;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _crookDetector = GetComponent<CrookDetector>();
        _updateDelay = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        _crookDetector.Enable += Enable;
        _crookDetector.Disable += Disable;
    }

    private void Start()
    {
        _source.clip = _clip;
        _source.loop = true;
    }

    private void OnDisable()
    {
        _crookDetector.Enable -= Enable;
        _crookDetector.Disable -= Disable;
    }

    private IEnumerator Broadcasting()
    {
        float delta = _maxVolume * Time.fixedDeltaTime / _reachMaxSeconds;
        float volume = 0;

        _isPlaySound = true;
        _source.Play();

        while (_isPlaySound)
        {
            if (_isWork)
                volume = Mathf.MoveTowards(volume, _maxVolume, delta);
            else
                volume = Mathf.MoveTowards(volume, 0, delta);

            volume = Mathf.Clamp(volume, 0, _maxVolume);

            _source.volume = volume;

            if (volume > 0)
                yield return _updateDelay;
            else
                _isPlaySound = false;
        }

        _source.Stop();
    }

    private void Enable()
    {
        if (_isWork)
            return;

        _isWork = true;

        if (_isPlaySound)
            return;

        StartCoroutine(Broadcasting());
    }

    private void Disable()
    {
        _isWork = false;
    }
}