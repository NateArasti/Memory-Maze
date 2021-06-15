using UnityEngine;

public class MouseLook : MonoBehaviour
{
#pragma warning disable 649
	[SerializeField] private Transform playerBody;

	public static float MouseSensitivity;
	public static int FieldOfView;

	private float _xRotation;
	private Camera _cam;

	private void Start()
	{
		MouseSensitivity = PlayerPrefs.HasKey("MouseSensitivity") ? PlayerPrefs.GetInt("MouseSensitivity") : 500;
		transform.localRotation = Quaternion.Euler(0, 0, 0);
		_cam = GetComponent<Camera>();
		_cam.fieldOfView = FieldOfView;
	}

	private void FixedUpdate()
	{
		_cam.fieldOfView = FieldOfView;
		var rotationX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.fixedDeltaTime;
		var rotationY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.fixedDeltaTime;

		_xRotation -= rotationY;
		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
		playerBody.Rotate(Vector3.up * rotationX);
	}
}