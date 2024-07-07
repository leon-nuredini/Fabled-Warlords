using System;
using System.Collections.Generic;
using Singleton;
using TbsFramework.Cells;
using UnityEngine;

public class PathPainter : SceneSingleton<PathPainter>
{
    [SerializeField] private LinkedList[] _linkedList;

    public void UpdateLinkedList(List<Cell> cellList)
    {
        DeletePath();
        _linkedList = new LinkedList[cellList.Count];
        for (int i = 0; i < _linkedList.Length; i++)
        {
            Node node = new Node();
            node.lSquare = cellList[i] as LSquare;

            int previousIndex = i - 1;
            if (previousIndex >= 0)
            {
                Node previousNode = _linkedList[previousIndex].node;
                previousNode.next = node;
                node.previous     = previousNode;
            }

            _linkedList[i] = new LinkedList(node);
        }

        for (int i = 0; i < _linkedList.Length; i++)
            GetPathDirection(_linkedList[i]);
    }

    private void GetPathDirection(LinkedList _linkedList)
    {
        LSquare previousTile = null;
        LSquare nextTile     = null;

        LSquare currTile = _linkedList.node.lSquare;
        if (_linkedList.node.previous != null)
            previousTile = _linkedList.node.previous.lSquare;
        if (_linkedList.node.next != null)
            nextTile = _linkedList.node.next.lSquare;

        if (previousTile == null)
        {
            currTile.PaintPath.DisableSpritePath();
            return;
        }

        PathType pathType = PathType.None;
        if (nextTile != null)
        {
            pathType = GetPath(currTile.transform.localPosition, nextTile.transform.localPosition);
            currTile.PaintPath.DrawPath(pathType);
        }
        else if (previousTile != null)
        {
            pathType = GetPath(previousTile.transform.localPosition, currTile.transform.localPosition);
            currTile.PaintPath.DrawPath(pathType);
        }
    }

    private PathType GetPath(Vector3 currTilePos, Vector3 destTilePos)
    {
        Vector3 direction = (currTilePos - destTilePos).normalized;
        direction.x = Mathf.RoundToInt(direction.x);
        direction.y = Mathf.RoundToInt(direction.y);
        direction.z = Mathf.RoundToInt(direction.z);

        PathType pathType              = PathType.None;
        if (direction.x > 0f) pathType = PathType.Left;
        if (direction.x < 0f) pathType = PathType.Right;
        if (direction.y > 0f) pathType = PathType.Down;
        if (direction.y < 0f) pathType = PathType.Up;

        return pathType;
    }

    public void DeletePath()
    {
        if (_linkedList == null || _linkedList.Length <= 0) return;
        for (int i = 0; i < _linkedList.Length; i++)
        {
            if (_linkedList[i] != null && _linkedList[i].node != null && _linkedList[i].node.lSquare != null)
                _linkedList[i].node.lSquare.PaintPath.DrawPath(PathType.None);
        }

        _linkedList = null;
    }
}