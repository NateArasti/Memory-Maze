using UnityEngine;

public class MusicHandler : MonoBehaviour
{
	[SerializeField] private AudioClip[] audioClips;

	private AudioSource _audioSource;
	public static MusicHandler Instance { get; private set; }

	private void Awake()
	{
		if (!Instance)
			Instance = this;
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