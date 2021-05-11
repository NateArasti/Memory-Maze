using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Set in Inspector")] public AudioClip[] audioClips;

    private AudioSource _audioSource;
    private static BackgroundMusic _instance;

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!_audioSource.isPlaying) _audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }
}