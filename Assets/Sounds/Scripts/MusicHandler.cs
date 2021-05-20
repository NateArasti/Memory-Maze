using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] audioClips;

    private AudioSource audioSource;
    public static MusicHandler Instance;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }
}