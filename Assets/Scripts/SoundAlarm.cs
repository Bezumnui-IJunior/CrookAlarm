using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundAlarm : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private CrookDetector _crookDetector;
    [SerializeField] private float _reachMaxSeconds;
    [SerializeField] private float _maxVolume = 1;

    private AudioSource _source;
    private WaitForFixedUpdate _updateDelay;
    private Coroutine _coroutine;

    private float _volume;
    private float _deltaVolume;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _updateDelay = new WaitForFixedUpdate();
        _deltaVolume = _maxVolume * Time.fixedDeltaTime / _reachMaxSeconds;
    }

    private void OnEnable()
    {
        _crookDetector.Detected += OnDetected;
        _crookDetector.Released += OnReleased;
    }

    private void Start()
    {
        _source.clip = _clip;
        _source.loop = true;
    }

    private void OnDisable()
    {
        _crookDetector.Detected -= OnDetected;
        _crookDetector.Released -= OnReleased;
    }

    private IEnumerator ReachingVolume(float target)
    {
        while (_volume > 0)
        {
            _source.volume = Mathf.MoveTowards(_source.volume, target, _deltaVolume);

            yield return _updateDelay;
        }

        _source.Stop();
    }

    private void OnDetected()
    {
        RestartCoroutine(ReachingVolume(_maxVolume));
    }

    private void OnReleased()
    {
        RestartCoroutine(ReachingVolume(0));
    }

    private void RestartCoroutine(IEnumerator coroutine)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(coroutine);
    }
}