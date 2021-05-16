using UnityEngine;

public class PlayerUI : MonoBehaviour
{
#pragma warning disable 649
    [Header("UI")]
    [SerializeField] private Camera camera3D;
    [SerializeField] private Camera camera2D;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1 - Time.timeScale;
        }
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
}