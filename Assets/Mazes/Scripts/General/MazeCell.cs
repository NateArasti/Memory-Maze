using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject upperWall;
    [SerializeField] private GameObject rightWall;

    public GameObject BottomWall => bottomWall;
    public GameObject LeftWall => leftWall;
    public GameObject UpperWall => upperWall;
    public GameObject RightWall => rightWall;
}
