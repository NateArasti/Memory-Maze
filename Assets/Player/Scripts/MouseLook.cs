using UnityEngine;

public class MouseLook : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    private float xRotation;

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        var rotationX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        var rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        xRotation -= rotationY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * rotationX);
    }
}
