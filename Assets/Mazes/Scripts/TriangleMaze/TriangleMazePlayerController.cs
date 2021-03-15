using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleMazePlayerController : PlayerController
{
    protected override Vector3 PlayerStartPos { get; } = new Vector3(0.5f, -49.7f, 3);

    protected override float AnimationDuration => 0.5f;

    protected override void PrepareForFirstMove()
    {
        transform.position = new Vector3(0, 2, 46);
        transform.eulerAngles = Vector3.zero;
        Camera3D.transform.localPosition = Vector3.zero;
        Camera3D.transform.eulerAngles = Vector3.zero;
    }

    protected override Vector3 GetPlayer2DPosition(Vector3 standardPosition) =>
        new Vector3(standardPosition.x / 2f, 
            0.866f * standardPosition.y + ((standardPosition.x + standardPosition.y) % 2 == 0 ? 0.3f : 0.5f), 0);

    protected override IEnumerator MovementTowards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);
        var X = CurrentNode.X;
        var Y = CurrentNode.Y;
        foreach (var t in path)
        {
            if ((X + Y) % 2 == 0)
            {
                if(Cells[X,Y].PreviousDirection == Vector2.right)
                {
                    if(t == Vector2.right)
                        yield return StartCoroutine(Move("Go60Left"));
                    else if(t == Vector2.down)
                        yield return StartCoroutine(Move("Go60Right"));
                }
                else if(Cells[X, Y].PreviousDirection == Vector2.left)
                {
                    if (t == Vector2.left)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (t == Vector2.down)
                        yield return StartCoroutine(Move("Go60Left"));
                }
                else
                {
                    if (t == Vector2.right)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (t == Vector2.left)
                        yield return StartCoroutine(Move("Go60Left"));
                }
            }
            else
            {
                if (Cells[X, Y].PreviousDirection == Vector2.right)
                {
                    if (t == Vector2.right)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (t == Vector2.up)
                        yield return StartCoroutine(Move("Go60Left"));
                }
                else if (Cells[X, Y].PreviousDirection == Vector2.left)
                {
                    if (t == Vector2.up)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (t == Vector2.left)
                        yield return StartCoroutine(Move("Go60Left"));
                }
                else
                {
                    if (t == Vector2.left)
                        yield return StartCoroutine(Move("Go60Right"));
                    else if (t == Vector2.right)
                        yield return StartCoroutine(Move("Go60Left"));
                }
            }
            X += t.x;
            Y += t.y;
        }
        if(MazeSpawner.Maze.FinishPosition.X == X && MazeSpawner.Maze.FinishPosition.Y == Y)
        {
            yield return StartCoroutine(Finish(Cells[X, Y]));
            yield break;
        }

        OffOrOnMoveButtons(true);
        OffOrOnSideButtons(true);
        CheckForPossibleDirections();
    }

    protected override IEnumerator Finish(MazeGeneratorCell cell)
    {
        if (cell.PreviousDirection == Vector2.right)
        {
            if (!cell.RightWall)
                yield return StartCoroutine(Move("Go60Left"));
            else if (!cell.BottomWall)
                yield return StartCoroutine(Move("Go60Right"));
        }
        else if (cell.PreviousDirection == Vector2.left)
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
        Score.EndGame(10 * (MazeSpawner.width + MazeSpawner.height + Nodes.Count), int.Parse(TotalScore.text), "TriangleMaze");
    }

    protected override IEnumerator MovementBackwards(List<Vector2Int> path)
    {
        OffOrOnMoveButtons(false);
        OffOrOnSideButtons(false);

        var x = CurrentNode.X - path[path.Count - 1].x;
        var y = CurrentNode.Y - path[path.Count - 1].y;
        for (var i = path.Count - 1; i >= 0; --i) 
        {
            if ((x + y) % 2 == 0)
            {
                if (Cells[x, y].PreviousDirection == Vector2.right)
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                    else if (path[i] == Vector2.down)
                        yield return StartCoroutine(Move("Go60BackRight"));
                }
                else if (Cells[x, y].PreviousDirection == Vector2.left)
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
                if (Cells[x, y].PreviousDirection == Vector2.right)
                {
                    if (path[i] == Vector2.right)
                        yield return StartCoroutine(Move("Go60BackRight"));
                    else if (path[i] == Vector2.up)
                        yield return StartCoroutine(Move("Go60BackLeft"));
                }
                else if (Cells[x, y].PreviousDirection == Vector2.left)
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
            x -= path[i - 1].x;
            y -= path[i - 1].y;
        }
        OffOrOnMoveButtons(true);
        OffOrOnSideButtons(true);
        CheckForPossibleDirections();
    }

    public override void MoveRight()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        switch (checkAngle)
        {
            case 0:
            case 60:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.right).Key);
                break;
            case 120:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.down).Key);
                break;
            case 300:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.up).Key);
                break;
            case 180:
            case 240:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.left).Key);
                break;
        }
    }

    public override void MoveLeft()
    {
        var checkAngle = Mathf.Round(transform.eulerAngles.y);
        switch (checkAngle)
        {
            case 0:
            case 300:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.left).Key);
                break;
            case 60:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.up).Key);
                break;
            case 270:
            case 240:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.down).Key);
                break;
            case 180:
            case 120:
                MoveToNextNode(Nodes[CurrentNode].First(node => node.Value[0] == Vector2Int.right).Key);
                break;
        }
    }

    protected override void CheckForPossibleDirections()
    {
        _BackButton.SetActive(CurrentNode != StartPosition);
        var previousDirection = CurrentNode.PreviousDirection;
        if ((CurrentNode.X + CurrentNode.Y) % 2 == 0)
        {
            if (previousDirection == Vector2.right)
            {
                if (CurrentNode.BottomWall)
                    _RightButton.SetActive(false);
                if (CurrentNode.RightWall)
                    _LeftButton.SetActive(false);
            }
            else if (previousDirection == Vector2.left)
            {
                if (CurrentNode.BottomWall)
                    _LeftButton.SetActive(false);
                if (CurrentNode.LeftWall)
                    _RightButton.SetActive(false);
            }
            else if (previousDirection == Vector2.up)
            {
                if (CurrentNode.LeftWall)
                    _LeftButton.SetActive(false);
                if (CurrentNode.RightWall)
                    _RightButton.SetActive(false);
            }
        }
        else
        {
            if (previousDirection == Vector2.right)
            {
                if (CurrentNode.BottomWall)
                    _LeftButton.SetActive(false);
                if (CurrentNode.RightWall)
                    _RightButton.SetActive(false);
            }
            else if (previousDirection == Vector2.left)
            {
                if (CurrentNode.BottomWall)
                    _RightButton.SetActive(false);
                if (CurrentNode.LeftWall)
                    _LeftButton.SetActive(false);
            }
            else if (previousDirection == Vector2.down)
            {
                if (CurrentNode.LeftWall)
                    _RightButton.SetActive(false);
                if (CurrentNode.RightWall)
                    _LeftButton.SetActive(false);
            }
        }
    }
}