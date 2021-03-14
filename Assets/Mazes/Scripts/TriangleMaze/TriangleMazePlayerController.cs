using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMazePlayerController : PlayerController
{
    protected override Vector3 playerStartPos { get; } = new Vector3(0.5f, -49.7f, 3);

    protected override float animationDuration => 0.5f;

    protected override void PrepareForFirstMove()
    {
        transform.position = new Vector3(0, 2, 46);
        transform.eulerAngles = Vector3.zero;
        Camera3D.transform.localPosition = Vector3.zero;
        Camera3D.transform.eulerAngles = Vector3.zero;
    }

    protected override Vector3 GetPlayer2DPosition(Vector3 standardPosition) =>
        new Vector3(standardPosition.x / 2f, 
            0.866f * standardPosition.y + ((standardPosition.x + standardPosition.y) % 2 == 0 ? 0.3f : 0.5f), 
            0);

    protected override IEnumerator MovementTowards(List<Vector2Int> path)
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
            yield return StartCoroutine(Finish(cells[X, Y]));
            yield break;
        }

        OffOrOnMoveButtons(true);
        OffOrOnSideButtons(true);
        CheckForPossibleDirections();
    }

    protected override IEnumerator Finish(MazeGeneratorCell cell)
    {
        if (cell.prevDir == Vector2.right)
        {
            if (!cell.RightWall)
                yield return StartCoroutine(Move("Go60Left"));
            else if (!cell.BottomWall)
                yield return StartCoroutine(Move("Go60Right"));
        }
        else if (cell.prevDir == Vector2.left)
        {
            if (!cell.LeftWall)
                yield return StartCoroutine(Move("Go60Right"));
            else if (!cell.BottomWall)
                yield return StartCoroutine(Move("Go60Left"));
        }
        else
        {
            if (!cell.RightWall)
                yield return StartCoroutine(Move("Go60Right"));
            else if (!cell.LeftWall)
                yield return StartCoroutine(Move("Go60Left"));
        }
        WinEndGame.SetActive(true);
        Score.EndGame(10 * (MazeSpawner.width + MazeSpawner.height + nodes.Count), int.Parse(TotalScore.text), "TriangleMaze");
    }

    protected override IEnumerator MovementBackwards(List<Vector2Int> path)
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

    public override void MoveRight()
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

    public override void MoveLeft()
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

    protected override void CheckForPossibleDirections()
    {
        if (curNode == startPosition)
        {
            _BackButton.SetActive(false);
        }
        else
            _BackButton.SetActive(true);
        var prevDir = curNode.prevDir;
        if ((curNode.X + curNode.Y) % 2 == 0)
        {
            if (prevDir == Vector2.right)
            {
                if (curNode.BottomWall)
                    _RightButton.SetActive(false);
                if (curNode.RightWall)
                    _LeftButton.SetActive(false);
            }
            else if (prevDir == Vector2.left)
            {
                if (curNode.BottomWall)
                    _LeftButton.SetActive(false);
                if (curNode.LeftWall)
                    _RightButton.SetActive(false);
            }
            else if (prevDir == Vector2.up)
            {
                if (curNode.LeftWall)
                    _LeftButton.SetActive(false);
                if (curNode.RightWall)
                    _RightButton.SetActive(false);
            }
        }
        else
        {
            if (prevDir == Vector2.right)
            {
                if (curNode.BottomWall)
                    _LeftButton.SetActive(false);
                if (curNode.RightWall)
                    _RightButton.SetActive(false);
            }
            else if (prevDir == Vector2.left)
            {
                if (curNode.BottomWall)
                    _RightButton.SetActive(false);
                if (curNode.LeftWall)
                    _LeftButton.SetActive(false);
            }
            else if (prevDir == Vector2.down)
            {
                if (curNode.LeftWall)
                    _RightButton.SetActive(false);
                if (curNode.RightWall)
                    _LeftButton.SetActive(false);
            }
        }
    }
}