using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Set in Inspector")]
    public AudioClip[] audioClips;

    private AudioSource audioSource;
    private static BackgroundMusic _instance;

    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        }
    }
}
