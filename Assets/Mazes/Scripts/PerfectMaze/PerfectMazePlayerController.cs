using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerfectMazePlayerController : PlayerController
{
    protected override Vector3 PlayerStartPos { get; } = new Vector3(0.7f, -49.2f, 3);

    protected override float AnimationDuration => 0.75f;

    protected override void PrepareForFirstMove()
    {
        var firstMove = Nodes[StartPosition].First().Value[0];
        Cells[0, 0].PreviousDirection = firstMove;
        transform.eulerAngles = new Vector3(0, Vector2.Angle(Vector2.up, firstMove), 0);
        transform.position -= new Vector3(CellSize3D.x / 2 * firstMove.x, 0, CellSize3D.z / 2 * firstMove.y);
    }

    protected override Vector3 GetPlayer2DPosition(Vector3 standardPosition)
    {
        return standardPosition;
    }

    protected override IEnumerator MovementTowards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);
        var x = CurrentNode.X;
        var y = CurrentNode.Y;
        foreach (var t in path)
        {
            if (Cells[x, y].PreviousDirection == t)
                yield return StartCoroutine(Move("GoForward"));
            else if (Cells[x, y].PreviousDirection == Vector2Int.up && t == Vector2Int.right ||
                     Cells[x, y].PreviousDirection == Vector2Int.right && t == Vector2Int.down ||
                     Cells[x, y].PreviousDirection == Vector2Int.down && t == Vector2Int.left ||
                     Cells[x, y].PreviousDirection == Vector2Int.left && t == Vector2Int.up)
                yield return StartCoroutine(Move("GoRight"));
            else
                yield return StartCoroutine(Move("GoLeft"));
            x += t.x;
            y += t.y;
        }

        if (MazeSpawner.Maze.FinishPosition.X == x && MazeSpawner.Maze.FinishPosition.Y == y)
        {
            yield return StartCoroutine(Finish(Cells[x, y]));
            Score.EndGame(10 * (MazeSpawner.Width + MazeSpawner.Height + Nodes.Count), int.Parse(TotalScore.text),
                "PerfectMaze");
            WinEndGame.SetActive(true);
            yield break;
        }

        OffOrOnSideButtons(true);
        OffOrOnMoveButtons(true);
        CheckForPossibleDirections();
    }

    protected override IEnumerator MovementBackwards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);
        var length = path.Count;
        var x = CurrentNode.X - path[length - 1].x;
        var y = CurrentNode.Y - path[length - 1].y;
        for (var i = length - 1; i >= 0; --i)
        {
            if (Cells[x, y].PreviousDirection == path[i])
                yield return StartCoroutine(Move("GoBack"));
            else if (Cells[x, y].PreviousDirection == Vector2Int.up && path[i] == Vector2Int.right ||
                     Cells[x, y].PreviousDirection == Vector2Int.right && path[i] == Vector2Int.down ||
                     Cells[x, y].PreviousDirection == Vector2Int.down && path[i] == Vector2Int.left ||
                     Cells[x, y].PreviousDirection == Vector2Int.left && path[i] == Vector2Int.up)
                yield return StartCoroutine(Move("GoBackAndRight"));
            else
                yield return StartCoroutine(Move("GoBackAndLeft"));
            if (i == 0) break;
            x -= path[i - 1].x;
            y -= path[i - 1].y;
        }

        OffOrOnSideButtons(true);
        OffOrOnMoveButtons(true);
        CheckForPossibleDirections();
    }

    protected override IEnumerator Finish(MazeGeneratorCell cell)
    {
        var x = cell.X;
        var y = cell.Y;
        var currentDirection = cell.PreviousDirection;
        Vector2Int finishDirection;
        if (x == 0)
            finishDirection = Vector2Int.left;
        else if (y == 0)
            finishDirection = Vector2Int.down;
        else if (x == MazeSpawner.Width - 1)
            finishDirection = Vector2Int.right;
        else
            finishDirection = Vector2Int.up;
        if (new Vector2Int(currentDirection.y, -currentDirection.x) == finishDirection)
            yield return StartCoroutine(Move("GoRight"));
        else if (new Vector2Int(-currentDirection.y, currentDirection.x) == finishDirection)
            yield return StartCoroutine(Move("GoLeft"));
        else
            yield return StartCoroutine(Move("GoForward"));

        yield return StartCoroutine(Move("GoForward"));
    }

    public void MoveForward()
    {
        Move(0);
    }

    public override void MoveRight()
    {
        Move(90);
    }

    public override void MoveLeft()
    {
        Move(270);
    }

    private void Move(float dAngle)
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        switch ((checkAngle + dAngle) % 360)
        {
            case 0:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2.up).Key);
                break;
            case 90:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2.right).Key);
                break;
            case 180:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2.down).Key);
                break;
            case 270:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2.left).Key);
                break;
        }
    }

    protected override void CheckForPossibleDirections()
    {
        var goUp = false;
        var goLeft = false;
        var goRight = false;
        var goDown = false;

        foreach (var nextNode in Nodes[CurrentNode])
            if (nextNode.Value[0] == Vector2.up)
                goUp = true;
            else if (nextNode.Value[0] == Vector2.down)
                goDown = true;
            else if (nextNode.Value[0] == Vector2.left)
                goLeft = true;
            else if (nextNode.Value[0] == Vector2.right)
                goRight = true;

        EnableButtons(goUp, goLeft, goRight, goDown);
    }

    private void EnableButtons(bool goUp, bool goLeft, bool goRight, bool goDown)
    {
        var checkVector = CurrentNode.PreviousDirection;
        if (CurrentNode == StartPosition)
        {
            _BackButton.SetActive(false);
            checkVector = Nodes[CurrentNode].First().Value[0];
        }
        else
        {
            _BackButton.SetActive(true);
        }

        if (checkVector == Vector2.up)
        {
            _ForwardButton.SetActive(goUp);
            _RightButton.SetActive(goRight);
            _LeftButton.SetActive(goLeft);
        }
        else if (checkVector == Vector2.right)
        {
            _ForwardButton.SetActive(goRight);
            _RightButton.SetActive(goDown);
            _LeftButton.SetActive(goUp);
        }
        else if (checkVector == Vector2.left)
        {
            _ForwardButton.SetActive(goLeft);
            _RightButton.SetActive(goUp);
            _LeftButton.SetActive(goDown);
        }
        else if (checkVector == Vector2.down)
        {
            _ForwardButton.SetActive(goDown);
            _RightButton.SetActive(goLeft);
            _LeftButton.SetActive(goRight);
        }
    }
}