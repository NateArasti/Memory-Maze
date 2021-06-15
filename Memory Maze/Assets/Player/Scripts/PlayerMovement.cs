using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649
	[Header("Timer")] [SerializeField] private Timer timer;

	[Header("Maze")] [SerializeField] private MazeStarter mazeStarter;

	[Header("Movement")] [SerializeField] private float speed = 10f;

	[SerializeField] private float jumpHeight = 3f;
	[SerializeField] private Transform groundChecker;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private float groundDistance = 0.3f;
	[SerializeField] private float gravity = -9.81f;

	private CharacterController _controller;
	private Vector3 _velocity = Vector3.zero;
	private bool _isGrounded;

	private void Start()
	{
		var maze = mazeStarter.CurrentMaze;
		_controller = GetComponent<CharacterController>();
		_controller.enabled = false;
		transform.position = maze.StartCell.Cell3DPosition + new Vector3(0, 2, 0);
		_controller.enabled = true;
		timer.SetTimer(maze.FinishCell.DistanceFromStart, maze);
	}

	private void FixedUpdate()
	{
		if (timer.TimerSet)
			Move();
	}

	private void Move()
	{
		_isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
		if (_isGrounded)
		{
			if (_velocity.y < 0)
				_velocity = Vector3.zero;
			if (Input.GetAxis("Jump") > 0)
				_velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
		}
		else
		{
			_velocity.y += gravity * Time.fixedDeltaTime;
		}

		var x = Input.GetAxis("Horizontal");
		var z = Input.GetAxis("Vertical");

		timer.TimerStarted = x != 0 || z != 0;
		var newSpeed = speed;
		timer.IsSpeedUp = false;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			newSpeed *= 2;
			timer.IsSpeedUp = true;
		}

		var transform1 = transform;
		var movement = transform1.right * x + transform1.forward * z;
		_controller.Move(movement * (newSpeed * Time.fixedDeltaTime));

		_controller.Move(_velocity * Time.fixedDeltaTime);
	}
}