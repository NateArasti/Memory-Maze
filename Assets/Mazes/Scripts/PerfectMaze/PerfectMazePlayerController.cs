using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerfectMazePlayerController : PlayerController
{
    protected override Vector3 playerStartPos { get; } = new Vector3(0.7f, -49.2f, 3);

    protected override float animationDuration => 0.75f;

    protected override void PrepareForFirstMove()
    {
        var firstMove = nodes[startPosition].First().Value[0];
        cells[0, 0].prevDir = firstMove;
        transform.eulerAngles =
            new Vector3(0, Vector2.Angle(Vector2.up, firstMove), 0);
        transform.position -= new Vector3(cellSize3D.x / 2 * firstMove.x, 0, cellSize3D.z / 2 * firstMove.y);
    }

    protected override Vector3 GetPlayer2DPosition(Vector3 standardPosition) => standardPosition;

    protected override IEnumerator MovementTowards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);
        int X = curNode.X;
        int Y = curNode.Y;
        for (var i = 0; i < path.Count; ++i)
        {
            if (cells[X, Y].prevDir == path[i])
                yield return StartCoroutine(Move("GoForward"));
            else if (cells[X, Y].prevDir == Vector2Int.up && path[i] == Vector2Int.right ||
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
            yield return StartCoroutine(Finish(cells[X, Y]));
            Score.EndGame(10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count), int.Parse(TotalScore.text), "PerfectMaze");
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

    protected override IEnumerator Finish(MazeGeneratorCell cell)
    {
        var x = cell.X;
        var y = cell.Y;
        var currentDirection = cell.prevDir;
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

    public override void MoveRight()
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

    public override void MoveLeft()
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

    protected override void CheckForPossibleDirections()
    {
        bool goUp = false;
        bool goLeft = false;
        bool goRight = false;
        bool goDown = false;

        foreach (var nextNode in nodes[curNode])
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
            _BackButton.SetActive(false);
            checkVector = nodes[curNode].First().Value[0];
        }
        else
            _BackButton.SetActive(true);
        
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