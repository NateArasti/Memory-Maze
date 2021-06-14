using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
#pragma warning disable 649
    [Header("Cameras")]
    [SerializeField] private Camera camera3D;
    [SerializeField] private Camera camera2D;

    [Header("Pause")] 
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button settingsPanelExitButton;

    [Header("Stories")] 
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private Story story;
    [SerializeField] private Timer timer;

    [Header("Finish")] 
    [SerializeField] private UnityEvent arcadeLose;
    [SerializeField] private UnityEvent arcadeWin;
    [SerializeField] private UnityEvent classicLose;
    [SerializeField] private UnityEvent classicWin;
    [SerializeField] private UnityEvent tutorialEnd;

    private bool IsPauseUnavailable => camera2D.enabled || storyPanel.activeInHierarchy || _mazeEnded;

    private int _addTimerValue;
    private LineRenderer _lineRenderer;
    private readonly Vector3 _delta = new Vector3(0, 0, 2);
    private bool _mazeEnded;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && camera2D.enabled) Draw();
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchPause();
    }

    public void SwitchPause()
    {
        if(IsPauseUnavailable) return;
        Time.timeScale = 1 - Time.timeScale;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        if((int)Time.timeScale == 1)
            settingsPanelExitButton.onClick.Invoke();
        pausePanel.SetActive(Time.timeScale == 0);
    }
    
    public void OffPause()
    {
        Time.timeScale = 1 - Time.timeScale;
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(false);
    }

    private void Draw()
    {
        _lineRenderer.positionCount += 1;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,
            camera2D.ScreenToWorldPoint(Input.mousePosition) + _delta);
    }

    public void SwapCams()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera2D.enabled = false;
        camera3D.enabled = true;
    }

    public void CollectStory(int distance)
    {
        var storyIndex = StoriesStorage.GetNotCollectedStory;
        StoriesStorage.CollectStory(storyIndex);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        storyPanel.SetActive(true);
        story.ShowStory(storyIndex);
        _addTimerValue = distance;
    }

    public void AddToTimer() => StartCoroutine(timer.AddTime(_addTimerValue));

    public void Win()
    {
        _mazeEnded = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if(MazeLoader.IsTutorial)
            tutorialEnd.Invoke();
        else if(ArcadeProgression.ProgressionOn)
            arcadeWin.Invoke();
        else
            classicWin.Invoke();
    }

    public void Lose()
    {
        _mazeEnded = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (ArcadeProgression.ProgressionOn)
            arcadeLose.Invoke();
        else
            classicLose.Invoke();
    }
}