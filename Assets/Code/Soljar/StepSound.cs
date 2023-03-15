using UnityEngine;

public class StepSound : MonoBehaviour
{

    [SerializeField]
    private AudioClip _steptSound;

    [SerializeField]
    private float _soundDelay = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void UpdateTimer()
    {
        if (_stepSoundDelay >= 0)
            _stepSoundDelay -= Time.deltaTime;
    }

    public bool IsReadyToSound()
    {
        if (_stepSoundDelay > 0)
            return false;

        _stepSoundDelay = _soundDelay;
        return true;
    }

    public void PlayStepSound()
    {
        _audioSource.volume = 0.1f;
        _audioSource.PlayOneShot(_steptSound);
    }

    private AudioSource _audioSource;
    private float _stepSoundDelay = 0.3f;
}
