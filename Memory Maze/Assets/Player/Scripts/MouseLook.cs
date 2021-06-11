using UnityEngine;

public class MouseLook : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Transform playerBody;

    public static float MouseSensitivity;
    public static int FieldOfView;

    private float xRotation;
    private Camera cam;

    private void Start()
    {
        MouseSensitivity = PlayerPrefs.HasKey("MouseSensitivity") ? PlayerPrefs.GetInt("MouseSensitivity") : 500;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        cam = GetComponent<Camera>();
        cam.fieldOfView = FieldOfView;
    }

    private void FixedUpdate()
    {
        cam.fieldOfView = FieldOfView;
        var rotationX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.fixedDeltaTime;
        var rotationY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.fixedDeltaTime;

        xRotation -= rotationY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * rotationX);
    }
}
