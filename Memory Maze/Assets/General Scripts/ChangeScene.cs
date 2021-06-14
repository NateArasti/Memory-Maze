using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
	private static ChangeScene _instance;
	private static bool _shouldPlayEndAnimation;
	public Image loadingProgressBar;

	private Animator _animator;
	private AsyncOperation _loadingSceneOperation;

	private void Start()
	{
		_instance = this;
		_animator = GetComponent<Animator>();

		if (!_shouldPlayEndAnimation)
		{
			gameObject.SetActive(false);
			return;
		}

		// ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
		_animator.SetTrigger("LoadingEnds");
		_instance.loadingProgressBar.fillAmount = 1;
		_shouldPlayEndAnimation = false;
	}

	private void Update()
	{
		if (_loadingSceneOperation == null) return;
		loadingProgressBar.fillAmount = Mathf.Lerp(loadingProgressBar.fillAmount, _loadingSceneOperation.progress,
			Time.deltaTime * 5);
	}

	public static void SwitchToScene(string sceneName, string oldScene)
	{
		_instance.gameObject.SetActive(true);
		_instance._animator.SetTrigger(oldScene == "Menu" ? "LoadingStartsFromMenu" : "LoadingStarts");
		_instance._loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
		_instance._loadingSceneOperation.allowSceneActivation = false;
		_instance.loadingProgressBar.fillAmount = 0;
	}

	public void OnAnimationOver()
	{
		_shouldPlayEndAnimation = true;
		_loadingSceneOperation.allowSceneActivation = true;
	}

	public void EndAnimation()
	{
		gameObject.SetActive(false);
	}
}