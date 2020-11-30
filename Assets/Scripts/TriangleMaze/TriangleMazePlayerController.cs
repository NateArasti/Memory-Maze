using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriangleMazePlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Camera Camera2D;
    public Camera Camera3D;
    public TriangleMazeHintRenderer HintRenderer;
    public TriangleMazeSpawner MazeSpawner;
    public GameObject Player2D;
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
    public Vector3 playerStartPos = new Vector3(0.5f, -50f, 3);

    private new Animation animation;
    private Dictionary<TriangleMazeGeneratorCell, Dictionary<TriangleMazeGeneratorCell, List<Vector2Int>>> nodes;
    private TriangleMazeGeneratorCell curNode;
    private TriangleMazeGeneratorCell startPosition;
    private TriangleMazeGeneratorCell[,] cells;
    private bool isMoved = false;
    private Score Score;
    private bool repeated = false;
    private bool hintShowed = false;

    void Start()
    {
        var maze = MazeSpawner.Maze;
        nodes = maze.nodes;
        startPosition = maze.startPosition;
        startPosition.prevDir = Vector2Int.up;
        cells = maze.cells;
        Player2D = Instantiate(Player2D, playerStartPos + 
            new Vector3(0, (startPosition.X + startPosition.Y) % 2 == 0 ? 0.3f : 0.5f, 0), Quaternion.identity);
        Player2D.transform.localScale = new Vector2(0.15f, 0.15f);

        TotalScore.text = (10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count)).ToString();
        Score = GetComponent<Score>();

        animation = Camera3D.GetComponent<Animation>();

        curNode = maze.startPosition;
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

        Player2D.transform.position = playerStartPos +
            new Vector3(0, (startPosition.X + startPosition.Y) % 2 == 0 ? 0.3f : 0.5f, 0);
        Player2D.transform.localScale = new Vector2(0.15f, 0.15f);

        transform.position = new Vector3(0, 2, 46);
        transform.eulerAngles = Vector3.zero;
        Camera3D.transform.localPosition = Vector3.zero;
        Camera3D.transform.eulerAngles = Vector3.zero;

        TotalScore.text = (10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count)).ToString();

        curNode = MazeSpawner.Maze.startPosition;
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

    private void MoveToNextNode(TriangleMazeGeneratorCell nextNode)
    {
        StartCoroutine(MovementTowards(nodes[curNode][nextNode]));
        isMoved = true;
        Vector2 newPos = new Vector2(nextNode.X, nextNode.Y);
        if (!nextNode.isDeadEnd)
            newPos -= nextNode.prevDir;
        Player2D.transform.position = playerStartPos + 
            new Vector3(newPos.x / 2f, 0.866f * newPos.y + ((newPos.x + newPos.y) % 2 == 0 ? 0.3f : 0.5f), 0);
        
        curNode = nextNode;
    }

    IEnumerator MovementTowards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);

        int X = curNode.X;
        int Y = curNode.Y;
        for (var i = 0; i < path.Count; ++i) 
        {
            if ((X + Y) % 2 == 0)
            {
                if(cells[X,Y].prevDir == Vector2.right)
                {
                    if(path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60Left"));
                    else if(path[i] == Vector2.down)
                        yield return StartCoroutine(Move("Go60Right"));
                }
                else if(cells[X, Y].prevDir == Vector2.left)
                {
                    if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (path[i] == Vector2.down)
                        yield return StartCoroutine(Move("Go60Left"));
                }
                else
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60Left"));
                }
            }
            else
            {
                if (cells[X, Y].prevDir == Vector2.right)
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (path[i] == Vector2.up)
                        yield return StartCoroutine(Move("Go60Left"));
                }
                else if (cells[X, Y].prevDir == Vector2.left)
                {
                    if (path[i] == Vector2.up)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60Left"));
                }
                else
                {
                    if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60Left"));
                }
            }
            X += path[i].x;
            Y += path[i].y;
        }
        if(MazeSpawner.Maze.finishPosition.X == X && MazeSpawner.Maze.finishPosition.Y == Y)
        {
            if (cells[X, Y].prevDir == Vector2.right)
            {
                if (!cells[X, Y].RightWall)
                    yield return StartCoroutine(Move("Go60Left"));
                else if (!cells[X, Y].BottomWall)
                    yield return StartCoroutine(Move("Go60Right"));
            }
            else if (cells[X, Y].prevDir == Vector2.left)
            {
                if (!cells[X, Y].LeftWall)
                    yield return StartCoroutine(Move("Go60Right"));
                else if (!cells[X, Y].BottomWall)
                    yield return StartCoroutine(Move("Go60Left"));
            }
            else
            {
                if (!cells[X, Y].RightWall)
                    yield return StartCoroutine(Move("Go60Right"));
                else if (!cells[X, Y].LeftWall)
                    yield return StartCoroutine(Move("Go60Left"));
            }
            WinEndGame.SetActive(true);
            Score.EndGame(10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count), int.Parse(TotalScore.text), "TriangleMaze");
            yield break;
        }

        OffOrOnMoveButtons(true);
        OffOrOnSideButtons(true);
        CheckForPossibleDirections();
    }

    IEnumerator MovementBackwards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);

        int X = curNode.X - path[path.Count - 1].x;
        int Y = curNode.Y - path[path.Count - 1].y;
        for (var i = path.Count - 1; i >= 0; --i) 
        {
            if ((X + Y) % 2 == 0)
            {
                if (cells[X, Y].prevDir == Vector2.right)
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                    else if (path[i] == Vector2.down)
                        yield return StartCoroutine(Move("Go60BackRight"));
                }
                else if (cells[X, Y].prevDir == Vector2.left)
                {
                    if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60BackRight"));
                    else if (path[i] == Vector2.down)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                }
                else
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60BackRight"));
                    else if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                }
            }
            else
            {
                if (cells[X, Y].prevDir == Vector2.right)
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60BackRight"));
                    else if (path[i] == Vector2.up)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                }
                else if (cells[X, Y].prevDir == Vector2.left)
                {
                    if (path[i] == Vector2.up)
                        yield return StartCoroutine(Move("Go60BackRight"));
                    else if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                }
                else
                {
                    if (path[i] == Vector2.left)
                        yield return StartCoroutine(Move("Go60BackRight"));
                    else if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                }
            }
            if (i == 0) break;
            X -= path[i - 1].x;
            Y -= path[i - 1].y;
        }
        OffOrOnMoveButtons(true);
        OffOrOnSideButtons(true);
        CheckForPossibleDirections();
    }

    IEnumerator Move(string animname)
    {
        animation.PlayQueued(animname);
        yield return new WaitForSeconds(0.5f);
        ChangeTransforms();
    }

    public void MoveBack()
    {
        var previousNode = curNode.prevNode;
        StartCoroutine(MovementBackwards(nodes[previousNode][curNode]));

        StartCoroutine(ChangeScore(30));

        Vector2 newPos = new Vector2(previousNode.X, previousNode.Y);
        if (!previousNode.isDeadEnd)
            newPos -= previousNode.prevDir;
        Player2D.transform.position = playerStartPos +
            new Vector3(newPos.x / 2f, 0.866f * newPos.y + ((newPos.x + newPos.y) % 2 == 0 ? 0.3f : 0.5f), 0);

        curNode = previousNode;
    }

    public void MoveRight()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        if (checkAngle == 0 || checkAngle == 60)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2.right)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 120)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.down)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 300)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.up)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 180 || checkAngle == 240)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.left)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
    }

    public void MoveLeft()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        if (checkAngle == 0 || checkAngle == 300)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.left)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 60)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.up)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 270 || checkAngle == 240)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.down)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
        else if (checkAngle == 180 || checkAngle == 120)
        {
            foreach (var nextNode in nodes[curNode])
                if (nextNode.Value[0] == Vector2Int.right)
                {
                    MoveToNextNode(nextNode.Key);
                    break;
                }
        }
    }

    private void ChangeTransforms()
    {
        var sin = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
        var cos = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);
        var x = Camera3D.transform.localPosition.x;
        var y = Camera3D.transform.localPosition.y;
        var z = Camera3D.transform.localPosition.z;

        transform.position += new Vector3((float)System.Math.Round(cos * x + sin * z, 2), y, 
            (float)System.Math.Round(cos * z - sin * x, 2));
        transform.eulerAngles = new Vector3(0,
            Mathf.Round(transform.eulerAngles.y + Camera3D.transform.localEulerAngles.y), 0);
        Camera3D.transform.localPosition = Vector3.zero;
        Camera3D.transform.localEulerAngles = Vector3.zero;
    }

    private void CheckForPossibleDirections()
    {
        if (curNode == startPosition)
        {
            BackButton.SetActive(false);
        }
        else
            BackButton.SetActive(true);
        var prevDir = curNode.prevDir;
        if ((curNode.X + curNode.Y) % 2 == 0)
        {
            if (prevDir == Vector2.right)
            {
                if (curNode.BottomWall)
                    RightButton.SetActive(false);
                if (curNode.RightWall)
                    LeftButton.SetActive(false);
            }
            else if (prevDir == Vector2.left)
            {
                if (curNode.BottomWall)
                    LeftButton.SetActive(false);
                if (curNode.LeftWall)
                    RightButton.SetActive(false);
            }
            else if (prevDir == Vector2.up)
            {
                if (curNode.LeftWall)
                    LeftButton.SetActive(false);
                if (curNode.RightWall)
                    RightButton.SetActive(false);
            }
        }
        else
        {
            if (prevDir == Vector2.right)
            {
                if (curNode.BottomWall)
                    LeftButton.SetActive(false);
                if (curNode.RightWall)
                    RightButton.SetActive(false);
            }
            else if (prevDir == Vector2.left)
            {
                if (curNode.BottomWall)
                    RightButton.SetActive(false);
                if (curNode.LeftWall)
                    LeftButton.SetActive(false);
            }
            else if (prevDir == Vector2.down)
            {
                if (curNode.LeftWall)
                    RightButton.SetActive(false);
                if (curNode.RightWall)
                    LeftButton.SetActive(false);
            }
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
