using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _reachMaxSeconds;
    [SerializeField] private float _maxVolume = 1;

    private float _volume;
    private AudioSource _source;
    private bool _isWork;
    private bool _isPlaySound = true;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _source.clip = _clip;
        _source.loop = true;
    }

    private void FixedUpdate()
    {
        if (_isPlaySound == false)
            return;

        float delta = _maxVolume * Time.fixedDeltaTime / _reachMaxSeconds;

        if (_isWork)
            _volume = Mathf.MoveTowards(_volume, _maxVolume, delta);
        else
            _volume = Mathf.MoveTowards(_volume, 0, delta);

        _volume = Mathf.Clamp(_volume, 0, _maxVolume);

        _source.volume = _volume;

        if (_volume > 0) return;

        _source.Stop();
        _isPlaySound = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Crook _) == false)
            return;

        Enable();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Crook _) == false)
            return;

        Disable();
    }

    private void Enable()
    {
        _volume = 0;
        _source.Play();
        _isWork = true;
        _isPlaySound = true;
    }

    private void Disable()
    {
        _isWork = false;
    }
}