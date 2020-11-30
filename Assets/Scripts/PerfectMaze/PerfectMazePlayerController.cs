using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PerfectMazePlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Camera Camera2D;
    public Camera Camera3D;
    public PerfectMazeHintRenderer HintRenderer;
    public PerfectMazeSpawner MazeSpawner;
    public GameObject Player2D;
    public GameObject ForwardButton;
    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject BackButton;
    public GameObject HintButton;
    public GameObject MapButton;
    public GameObject ExitButton;
    public GameObject SettingsButton;
    public GameObject WinEndGame;
    public GameObject LoseEndGame;
    public Animation addAnim;
    public Text addScore;
    public Text TotalScore;
    public int cellFloorHeight = 2;
    public Vector3 playerStartPos = new Vector3(0.7f, -49.2f, 3);

    private new Animation animation;
    private Dictionary<PerfectMazeGeneratorCell, Dictionary<PerfectMazeGeneratorCell, List<Vector2Int>>> nodes;
    private PerfectMazeGeneratorCell curNode;
    private PerfectMazeGeneratorCell startPosition;
    private PerfectMazeGeneratorCell[,] cells;
    private Vector3 cellSize3D;
    private bool isMoved = false;
    private Score Score;
    private bool repeated = false;
    private bool hintShowed = false;

    private void Start()
    {
        var maze = MazeSpawner.Maze;
        nodes = maze.nodes;
        cells = maze.cells;
        startPosition = maze.startPosition;
        cellSize3D = MazeSpawner.cellSize3D;

        Player2D = Instantiate(Player2D, playerStartPos, Quaternion.identity);

        TotalScore.text = (10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count)).ToString();
        Score = GetComponent<Score>();

        animation = Camera3D.GetComponent<Animation>();
        
        curNode = maze.startPosition;

        var firstMove = nodes[startPosition].First().Value[0];
        cells[0, 0].prevDir = firstMove;
        transform.eulerAngles = 
            new Vector3(0, Vector2.Angle(Vector2.up, firstMove), 0);
        transform.position -= new Vector3(cellSize3D.x / 2 * firstMove.x, 0, cellSize3D.z / 2 * firstMove.y);
    }

    public void Repeat()
    {
        if (repeated) return;
        repeated = true;

        animation.Stop();
        StopAllCoroutines();
        LoseEndGame.SetActive(false);

        Camera3D.enabled = false;
        Camera2D.enabled = true;

        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(true);

        Player2D.transform.position = playerStartPos;

        TotalScore.text = (10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count)).ToString();

        curNode = MazeSpawner.Maze.startPosition;

        var firstMove = nodes[startPosition].First().Value[0];
        cells[0, 0].prevDir = firstMove;
        transform.eulerAngles =
            new Vector3(0, Vector2.Angle(Vector2.up, firstMove), 0);
        transform.position = new Vector3(cellSize3D.x / 2 * firstMove.x, 2, 50 -cellSize3D.z / 2 * firstMove.y);
    }

    IEnumerator ChangeScore(int add)
    {
        addScore.text = "-" + add;
        addAnim.Play();
        yield return new WaitForSeconds(0.5f);
        TotalScore.text = (int.Parse(TotalScore.text) - add).ToString();
        if (int.Parse(TotalScore.text) <= 0)
        {
            LoseEndGame.SetActive(true);
            if (repeated)
            {
                LoseEndGame.transform.GetChild(2).transform.localPosition = new Vector3(0, -50, 0);
                LoseEndGame.transform.GetChild(3).gameObject.SetActive(false);
            }
            HintButton.SetActive(false);
            OffOrOnMoveButtons(false);
            OffOrOnSideButtons(false);
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator MovementTowards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);
        int X = curNode.X;
        int Y = curNode.Y;
        for (var i = 0; i < path.Count; ++i)
        {
            if(cells[X,Y].prevDir == path[i])
                yield return StartCoroutine(Move("GoForward"));
            else if(cells[X, Y].prevDir == Vector2Int.up && path[i] == Vector2Int.right ||
                cells[X, Y].prevDir == Vector2Int.right && path[i] == Vector2Int.down ||
                cells[X, Y].prevDir == Vector2Int.down && path[i] == Vector2Int.left ||
                cells[X, Y].prevDir == Vector2Int.left && path[i] == Vector2Int.up)
                yield return StartCoroutine(Move("GoRight"));
            else
                yield return StartCoroutine(Move("GoLeft"));
            X += path[i].x;
            Y += path[i].y;
        }
        if (MazeSpawner.Maze.finishPosition.X == X && MazeSpawner.Maze.finishPosition.Y == Y)
        {
            yield return StartCoroutine(Finish(cells[X, Y].prevDir));
            Score.EndGame(10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count), int.Parse(TotalScore.text), "PerfectMaze");
            WinEndGame.SetActive(true);
            yield break;
        }
        OffOrOnSideButtons(true);
        OffOrOnMoveButtons(true);
        CheckForPossibleDirections();
    }

    IEnumerator MovementBackwards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);
        var length = path.Count;
        int X = curNode.X - path[length - 1].x;
        int Y = curNode.Y - path[length - 1].y;
        for (var i = length - 1; i >= 0; --i)
        {
            if (cells[X, Y].prevDir == path[i])
                yield return StartCoroutine(Move("GoBack"));
            else if (cells[X, Y].prevDir == Vector2Int.up && path[i] == Vector2Int.right ||
                cells[X, Y].prevDir == Vector2Int.right && path[i] == Vector2Int.down ||
                cells[X, Y].prevDir == Vector2Int.down && path[i] == Vector2Int.left ||
                cells[X, Y].prevDir == Vector2Int.left && path[i] == Vector2Int.up)
                yield return StartCoroutine(Move("GoBackAndRight"));
            else
                yield return StartCoroutine(Move("GoBackAndLeft"));
            if (i == 0) break;
            X -= path[i - 1].x;
            Y -= path[i - 1].y;
        }
        OffOrOnSideButtons(true);
        OffOrOnMoveButtons(true);
        CheckForPossibleDirections();
    }

    IEnumerator Move(string animname)
    {
        animation.PlayQueued(animname);
        yield return new WaitForSeconds(0.75f);
        ChangeTransforms();
    }

    IEnumerator Finish(Vector2Int currentDirection)
    {
        var x = MazeSpawner.Maze.finishPosition.X;
        var y = MazeSpawner.Maze.finishPosition.Y;
        Vector2Int finishDirection;
        if (x == 0)
            finishDirection = Vector2Int.left;
        else if (y == 0)
            finishDirection = Vector2Int.down;
        else if (x == MazeSpawner.width - 1)
            finishDirection = Vector2Int.right;
        else
            finishDirection = Vector2Int.up;
        if (new Vector2Int(currentDirection.y, -currentDirection.x) == finishDirection)
        {
            yield return StartCoroutine(Move("GoRight"));
        }
        else if (new Vector2Int(-currentDirection.y, currentDirection.x) == finishDirection)
        {
            yield return StartCoroutine(Move("GoLeft"));
        }
        else
        {
            yield return StartCoroutine(Move("GoForward"));
        }
        yield return StartCoroutine(Move("GoForward"));
    }

    private void ChangeTransforms()
    {
        var sin = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
        var cos = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);
        var x = Camera3D.transform.localPosition.x;
        var y = Camera3D.transform.localPosition.y;
        var z = Camera3D.transform.localPosition.z;
        
        transform.position += new Vector3(Mathf.Round(cos * x + sin * z), y, Mathf.Round(cos * z - sin * x));
        transform.eulerAngles = new Vector3(0, 
            Mathf.Round(transform.eulerAngles.y + Camera3D.transform.localEulerAngles.y), 0);
        Camera3D.transform.localPosition = Vector3.zero;
        Camera3D.transform.localEulerAngles = Vector3.zero;
    }

    private void MoveToNextNode(PerfectMazeGeneratorCell nextNode)
    {
        isMoved = true;
        var path = nodes[curNode][nextNode];
        StartCoroutine(MovementTowards(path));
        Vector2Int destination = new Vector2Int(nextNode.X, nextNode.Y);
        if (!nextNode.isDeadEnd)
            destination -= nextNode.prevDir;

        Player2D.transform.position = playerStartPos + new Vector3(destination.x, destination.y, 0);

        curNode = nextNode;
    }

    public void MoveBack()
    {
        StartCoroutine(ChangeScore(30));
        var previousNode = curNode.prevNode;
        StartCoroutine(MovementBackwards(nodes[previousNode][curNode]));

        Vector2Int destination = new Vector2Int(previousNode.X, previousNode.Y);
        if (previousNode != startPosition)
            destination -= previousNode.prevDir;
        Player2D.transform.position = playerStartPos + new Vector3(destination.x, destination.y, 0);

        curNode = previousNode;
    }

    public void MoveForward()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        if (checkAngle == 0)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.up)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 90)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.right)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 270)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.left)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 180)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.down)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
    }

    public void MoveRight()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        if (checkAngle == 0)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.right)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 90)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.down)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 270)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.up)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 180)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.left)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
    }

    public void MoveLeft()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        if (checkAngle == 0)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.left)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 90)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.up)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 270)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.down)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 180)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.right)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
    }

    private void CheckForPossibleDirections()
    {
        bool goUp = false;
        bool goLeft = false;
        bool goRight = false;
        bool goDown = false;

        foreach(var nextNode in nodes[curNode])
        {
            if (nextNode.Value[0] == Vector2.up)
                goUp = true;
            else if (nextNode.Value[0] == Vector2.down)
                goDown = true;
            else if (nextNode.Value[0] == Vector2.left)
                goLeft = true;
            else if (nextNode.Value[0] == Vector2.right)
                goRight = true;
        }

        EnableButtons(goUp, goLeft, goRight, goDown);
    }

    private void EnableButtons(bool goUp, bool goLeft, bool goRight, bool goDown)
    {
        var checkVector = curNode.prevDir;
        if (curNode == startPosition)
        {
            BackButton.SetActive(false);
            checkVector = nodes[curNode].First().Value[0];
        }
        else
            BackButton.SetActive(true);
        
        if (checkVector == Vector2.up)
        {
            ForwardButton.SetActive(goUp);
            RightButton.SetActive(goRight);
            LeftButton.SetActive(goLeft);
        }
        else if (checkVector == Vector2.right)
        {
            ForwardButton.SetActive(goRight);
            RightButton.SetActive(goDown);
            LeftButton.SetActive(goUp);
        }
        else if (checkVector == Vector2.left)
        {
            ForwardButton.SetActive(goLeft);
            RightButton.SetActive(goUp);
            LeftButton.SetActive(goDown);
        }
        else if (checkVector == Vector2.down)
        {
            ForwardButton.SetActive(goDown);
            RightButton.SetActive(goLeft);
            LeftButton.SetActive(goRight);
        }
    }

    public void ShowPath()
    {
        if (hintShowed) return;
        hintShowed = true;
        HintRenderer.DrawPath();
        StartCoroutine(ChangeScore(int.Parse(TotalScore.text) / 2));
        HintButton.SetActive(false);
    }

    public void SwapCams()
    {
        if (Camera3D.enabled == true)
        {
            if (isMoved) 
            {
                StartCoroutine(ChangeScore(50));
                isMoved = false;
            }
            Camera3D.enabled = false;
            Camera2D.enabled = true;

            OffOrOnMoveButtons(false);
            if (!hintShowed)
                HintButton.SetActive(true);
        }
        else
        {
            Camera2D.enabled = false;
            Camera3D.enabled = true;

            OffOrOnMoveButtons(true);
            HintButton.SetActive(false);

            CheckForPossibleDirections();
        }
    }

    private void OffOrOnMoveButtons(bool change)
    {
        ForwardButton.SetActive(change);
        RightButton.SetActive(change);
        LeftButton.SetActive(change);
        BackButton.SetActive(change);
    }

    private void OffOrOnSideButtons(bool change)
    {
        MapButton.SetActive(change);
        ExitButton.SetActive(change);
        SettingsButton.SetActive(change);
    }
}