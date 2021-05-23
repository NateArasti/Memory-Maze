using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Image loadingProgressBar;
    [SerializeField] private GameObject screen; 
    
    private static ChangeScene _instance;
    private static bool _shouldPlayOpeningAnimation; 
    
    private Animator animator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        _instance.animator.SetTrigger("NewSceneOpened");
        _instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _instance.loadingSceneOperation.allowSceneActivation = false;
        _instance.loadingProgressBar.fillAmount = 0;
    }
    
    private void Awake()
    {
        _instance = this;
        
        animator = GetComponent<Animator>();

        if (!_shouldPlayOpeningAnimation) return;
        animator.SetTrigger("NewSceneLoaded");
        _instance.loadingProgressBar.fillAmount = 1;
        _shouldPlayOpeningAnimation = false;
    }

    private void Update()
    {
        if (loadingSceneOperation == null) return;
        loadingProgressBar.fillAmount = Mathf.Lerp(loadingProgressBar.fillAmount, loadingSceneOperation.progress,
            Time.deltaTime * 5);
    }

    public void OnAnimationOver()
    {
        _shouldPlayOpeningAnimation = true;
        
        loadingSceneOperation.allowSceneActivation = true;
    }
    
    public void OffScreen()
    {
        screen.SetActive(false);
    }
}
