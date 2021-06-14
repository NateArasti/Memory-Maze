using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Image loadingProgressBar;
    
    private static ChangeScene _instance;
    private static bool _shouldPlayEndAnimation; 
    
    private Animator animator;
    private AsyncOperation loadingSceneOperation;

    private void Start()
    {
        _instance = this;
        animator = GetComponent<Animator>();

        if (!_shouldPlayEndAnimation)
        {
            gameObject.SetActive(false);
            return;
        }
        animator.SetTrigger("LoadingEnds");
        _instance.loadingProgressBar.fillAmount = 1;
        _shouldPlayEndAnimation = false;
    }
    
    public static void SwitchToScene(string sceneName, string oldScene)
    {
        _instance.gameObject.SetActive(true);
        _instance.animator.SetTrigger(oldScene == "Menu" ? "LoadingStartsFromMenu" : "LoadingStarts");
        _instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _instance.loadingSceneOperation.allowSceneActivation = false;
        _instance.loadingProgressBar.fillAmount = 0;
    }
    
    private void Update()
    {
        if (loadingSceneOperation == null) return;
        loadingProgressBar.fillAmount = Mathf.Lerp(loadingProgressBar.fillAmount, loadingSceneOperation.progress,
            Time.deltaTime * 5);
    }

    public void OnAnimationOver()
    {
        _shouldPlayEndAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
    }

    public void EndAnimation()
    {
        gameObject.SetActive(false);
    }
}
