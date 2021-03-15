using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] protected Camera Camera2D;
    [SerializeField] protected Camera Camera3D;
    [SerializeField] protected HintRenderer HintRenderer;
    [SerializeField] protected MazeSpawner MazeSpawner;
    [SerializeField] protected GameObject Player2D;
    [SerializeField] protected GameObject _ForwardButton;
    [SerializeField] protected GameObject _LeftButton;
    [SerializeField] protected GameObject _RightButton;
    [SerializeField] protected GameObject _BackButton;
    [SerializeField] protected GameObject HintButton;
    [SerializeField] protected GameObject MapButton;
    [SerializeField] protected GameObject ExitButton;
    [SerializeField] protected GameObject SettingsButton;
    [SerializeField] protected GameObject WinEndGame;
    [SerializeField] protected GameObject LoseEndGame;
    [SerializeField] protected Animation addAnim;
    [SerializeField] protected Text addScore;
    [SerializeField] protected Text TotalScore;
    [SerializeField] protected int cellFloorHeight = 2;
    protected abstract Vector3 PlayerStartPos { get; }
    protected abstract float AnimationDuration { get; }

    private Animation _animation;
    protected Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>> Nodes;
    protected MazeGeneratorCell CurrentNode;
    protected MazeGeneratorCell StartPosition;
    protected MazeGeneratorCell[,] Cells;
    protected Vector3 CellSize3D;
    private bool _isMoved;
    protected Score Score;
    private bool _repeated;
    private bool _hintShowed;

    protected void Start()
    {
        var maze = MazeSpawner.Maze;
        Nodes = maze.Nodes;
        Cells = maze.Cells;
        StartPosition = maze.StartPosition;
        CellSize3D = MazeSpawner.cellSize3D;

        Player2D = Instantiate(Player2D, PlayerStartPos + GetPlayer2DPosition(Vector3.zero), Quaternion.identity);

        TotalScore.text = (10 * (MazeSpawner.width + MazeSpawner.height + Nodes.Count)).ToString();
        Score = GetComponent<Score>();

        _animation = Camera3D.GetComponent<Animation>();

        CurrentNode = maze.StartPosition;

        PrepareForFirstMove();
    }

    protected abstract Vector3 GetPlayer2DPosition(Vector3 standardPosition);

    protected abstract void PrepareForFirstMove();

    protected void MoveToNextNode(MazeGeneratorCell nextNode)
    {
        _isMoved = true;
        StartCoroutine(MovementTowards(Nodes[CurrentNode][nextNode]));
        var destination = new Vector2(nextNode.X, nextNode.Y);
        if (!nextNode.IsDeadEnd)
            destination -= nextNode.PreviousDirection;

        Player2D.transform.position = PlayerStartPos + GetPlayer2DPosition(destination);

        CurrentNode = nextNode;
    }

    public void Repeat()
    {
        if (_repeated) return;
        _repeated = true;

        _animation.Stop();
        StopAllCoroutines();
        LoseEndGame.SetActive(false);

        Camera3D.enabled = false;
        Camera2D.enabled = true;

        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(true);

        Player2D.transform.position = PlayerStartPos;

        TotalScore.text = (10 * (MazeSpawner.width + MazeSpawner.height + Nodes.Count)).ToString();

        CurrentNode = MazeSpawner.Maze.StartPosition;

        PrepareForFirstMove();
    }

    private IEnumerator ChangeScore(int add)
    {
        addScore.text = "-" + add;
        addAnim.Play();
        yield return new WaitForSeconds(0.5f);
        TotalScore.text = (int.Parse(TotalScore.text) - add).ToString();
        if (int.Parse(TotalScore.text) > 0) yield break;
        LoseEndGame.SetActive(true);
        if (_repeated)
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

    protected abstract IEnumerator MovementTowards(List<Vector2Int> path);

    protected abstract IEnumerator MovementBackwards(List<Vector2Int> path);

    protected IEnumerator Move(string animationName)
    {
        _animation.PlayQueued(animationName);
        yield return new WaitForSeconds(AnimationDuration);
        ChangeTransforms();
    }

    protected abstract IEnumerator Finish(MazeGeneratorCell cell);

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

    public void MoveBack()
    {
        StartCoroutine(ChangeScore(30));
        var previousNode = CurrentNode.PreviousNode;
        StartCoroutine(MovementBackwards(Nodes[previousNode][CurrentNode]));

        var destination = new Vector2(previousNode.X, previousNode.Y);
        if (previousNode != StartPosition)
            destination -= previousNode.PreviousDirection;
        Player2D.transform.position = PlayerStartPos + GetPlayer2DPosition(destination);

        CurrentNode = previousNode;
    }

    public abstract void MoveRight();

    public abstract void MoveLeft();

    protected abstract void CheckForPossibleDirections();

    public void ShowPath()
    {
        if (_hintShowed) return;
        _hintShowed = true;
        HintRenderer.DrawPath();
        StartCoroutine(ChangeScore(int.Parse(TotalScore.text) / 2));
        HintButton.SetActive(false);
    }

    public void SwapCams()
    {
        if (Camera3D.enabled == true)
        {
            if (_isMoved)
            {
                StartCoroutine(ChangeScore(50));
                _isMoved = false;
            }
            Camera3D.enabled = false;
            Camera2D.enabled = true;

            OffOrOnMoveButtons(false);
            if (!_hintShowed)
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

    protected void OffOrOnMoveButtons(bool change)
    {
        if (_ForwardButton != null) _ForwardButton.SetActive(change);
        if (_RightButton != null) _RightButton.SetActive(change);
        if (_LeftButton != null) _LeftButton.SetActive(change);
        if (_BackButton != null) _BackButton.SetActive(change);
    }

    protected void OffOrOnSideButtons(bool change)
    {
        MapButton.SetActive(change);
        ExitButton.SetActive(change);
        SettingsButton.SetActive(change);
    }
}
