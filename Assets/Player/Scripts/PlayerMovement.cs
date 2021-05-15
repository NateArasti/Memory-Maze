using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649
    [Header("Timer")] 
    [SerializeField] private Timer timer;

    [Header("Maze")] 
    [SerializeField] private MazeStarter mazeStarter;

    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private float gravity = -9.81f;
    
    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded;
    private Maze maze;

    private void Start()
    {
        maze = mazeStarter.CurrentMaze;
        controller = GetComponent<CharacterController>();
        controller.enabled = false;
        transform.position = maze.StartCell.Cell3DPosition + new Vector3(0, 5, 0);
        controller.enabled = true;
        StartCoroutine(timer.SetTimer(maze.FinishCell.DistanceFromStart));
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        if (isGrounded)
        {
            if (velocity.y < 0)
                velocity = Vector3.zero;
            if (Input.GetAxis("Jump") > 0)
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        else velocity.y += gravity * Time.fixedDeltaTime;

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        timer.TimerStarted = x != 0 || z != 0;

        var newSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * 2f : speed;

        var movement = transform.right * x + transform.forward * z;
        controller.Move(movement * newSpeed * Time.fixedDeltaTime);

        controller.Move(velocity * Time.fixedDeltaTime);
    }
}
