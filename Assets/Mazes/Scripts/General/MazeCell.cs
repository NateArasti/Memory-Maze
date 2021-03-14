using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject upperWall;
    [SerializeField] private GameObject rightWall;

    public GameObject BottomWall { get => bottomWall; }
    public GameObject LeftWall { get => leftWall;}
    public GameObject UpperWall { get => upperWall; }
    public GameObject RightWall { get => rightWall; }
}
