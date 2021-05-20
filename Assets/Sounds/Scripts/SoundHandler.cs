using UnityEngine;

public class SoundHandler : MonoBehaviour
{
	private static SoundHandler _instance;

	private void Awake()
	{
		if (!_instance)
			_instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
}