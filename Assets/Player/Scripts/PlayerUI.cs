using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
#pragma warning disable 649
    [Header("Cameras")]
    [SerializeField] private Camera camera3D;
    [SerializeField] private Camera camera2D;

    [Header("UI Panels")] 
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject losePanelClassic;
    [SerializeField] private GameObject losePanelArcade;
    [SerializeField] private GameObject winPanelClassic;
    [SerializeField] private GameObject winPanelArcade;

    private LineRenderer lineRenderer;
    private readonly Vector3 delta = new Vector3(0, 0, 2);

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && camera2D.enabled) Draw();
        if (Input.GetKeyDown(KeyCode.M)) SwapCams();
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchPause();
    }

    public void SwitchPause()
    {
        Time.timeScale = 1 - Time.timeScale;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? 
            CursorLockMode.None : CursorLockMode.Locked;
        pausePanel.SetActive(Time.timeScale == 0);
    }

    private void Draw()
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1,
            camera2D.ScreenToWorldPoint(Input.mousePosition) + delta);
    }

    private void SwapCams()
    {
        if (camera2D.enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            camera2D.enabled = false;
            camera3D.enabled = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            camera2D.enabled = true;
            camera3D.enabled = false;
        }
    }

    public void Win()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (ArcadeProgression.ProgressionOn)
        {
            winPanelArcade.SetActive(true);
        }
        else 
            winPanelClassic.SetActive(true);
    }

    public void Lose()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (ArcadeProgression.ProgressionOn)
        {
            var mazeNumber = losePanelArcade.transform.GetChild(2).GetComponent<Text>();
            var words = mazeNumber.text.Split(' ');
            mazeNumber.text = $"{int.Parse(words[0]) + 1}" + words[1];
            losePanelArcade.SetActive(true);
        }
        else
            losePanelClassic.SetActive(true);
    }
}