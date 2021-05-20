using UnityEngine;
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
    [Header("Finish")]
    [SerializeField] private GameObject finishClassic;
    [SerializeField] private GameObject[] classicFinishHeaders;
    [SerializeField] private GameObject finishArcade;
    [SerializeField] private GameObject[] arcadeFinishHeaders;

    private LineRenderer lineRenderer;
    private readonly Vector3 delta = new Vector3(0, 0, 2);

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && camera2D.enabled) Draw();
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchPause();
    }

    public void SwitchPause()
    {
        if(camera2D.enabled) return;
        Time.timeScale = 1 - Time.timeScale;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        settingsPanelExitButton.onClick.Invoke();
        pausePanel.SetActive(Time.timeScale == 0);
    }

    private void Draw()
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1,
            camera2D.ScreenToWorldPoint(Input.mousePosition) + delta);
    }

    public void SwapCams()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera2D.enabled = false;
        camera3D.enabled = true;
    }

    public void Win()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (ArcadeProgression.ProgressionOn)
        {
            arcadeFinishHeaders[0].SetActive(true);
            finishArcade.SetActive(true);
        }
        else
        {
            classicFinishHeaders[0].SetActive(true);
            finishClassic.SetActive(true);
        }
    }

    public void Lose()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (ArcadeProgression.ProgressionOn)
        {
            arcadeFinishHeaders[1].SetActive(true);
            finishArcade.SetActive(true);
        }
        else
        {
            classicFinishHeaders[1].SetActive(true);
            finishClassic.SetActive(true);
        }
    }
}