/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyAStar (version 1.0)
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyClasses
{
    public class MyAStar
    {
        #region ----- Variable -----

        private ESearchOrder mSearchOder;
        private KeyValuePair<int, int>[] mMoveDirections = new KeyValuePair<int, int>[4];

        #endregion

        #region ----- Property -----

        public ESearchOrder SearchOrder
        {
            get { return mSearchOder; }
        }

        #endregion

        #region ----- Constructor -----

        public MyAStar(ESearchOrder searchOrder = ESearchOrder.LeftRightDownUp)
        {
            SetSearchOrder(searchOrder);
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Search a shortest valid path.
        /// </summary>
        public List<Vector2> Search(int[][] grid, int fromX, int fromY, int toX, int toY)
        {
            if (grid == null)
            {
                return null;
            }

            int maxX = grid.GetLength(0);
            if (maxX == 0)
            {
                return null;
            }

            int maxY = grid[0].Length;
            if (maxY == 0)
            {
                return null;
            }

            Dictionary<string, Node> checkingNodes = new Dictionary<string, Node>();
            Dictionary<string, Node> checkedNodes = new Dictionary<string, Node>();

            Node startNode = new Node { X = fromX, Y = fromY };
            string startKey = startNode.X + "." + startNode.Y;
            checkingNodes.Add(startKey, startNode);

            Func<KeyValuePair<string, Node>> _GetSmallestCheckingNode = () =>
            {
                KeyValuePair<string, Node> smallestNode = checkingNodes.ElementAt(0);
                foreach (KeyValuePair<string, Node> item in checkingNodes)
                {
                    if ((item.Value.F < smallestNode.Value.F)
                        || (item.Value.F == smallestNode.Value.F && item.Value.H < smallestNode.Value.H))
                    {
                        smallestNode = item;
                    }
                }

                return smallestNode;
            };

            while (true)
            {
                // don't have any open nodes
                if (checkingNodes.Count == 0)
                {
                    return null;
                }

                // find smallest value node
                KeyValuePair<string, Node> currentSmallestNode = _GetSmallestCheckingNode();

                // reach target point
                if (currentSmallestNode.Value.X == toX && currentSmallestNode.Value.Y == toY)
                {
                    List<Vector2> nodes = new List<Vector2>();
                    Node endNode = currentSmallestNode.Value;
                    while (endNode.X != startNode.X || endNode.Y != startNode.Y)
                    {
                        nodes.Insert(0, new Vector2(endNode.X, endNode.Y));
                        endNode = endNode.Parent;
                    }
                    nodes.Insert(0, new Vector2(endNode.X, endNode.Y));

                    return nodes;
                }

                // update checking status of nodes
                checkingNodes.Remove(currentSmallestNode.Key);
                checkedNodes.Add(currentSmallestNode.Key, currentSmallestNode.Value);

                // move to neighbour nodes
                foreach (KeyValuePair<int, int> moveDirection in mMoveDirections)
                {
                    // out of bounds
                    int curX = currentSmallestNode.Value.X + moveDirection.Key;
                    if (curX < 0 || maxX <= curX)
                    {
                        continue;
                    }

                    // out of bounds
                    int curY = currentSmallestNode.Value.Y + moveDirection.Value;
                    if (curY < 0 || maxY <= curY)
                    {
                        continue;
                    }

                    // obstacle
                    if (grid[curX][curY] <= 0)
                    {
                        continue;
                    }

                    // node is checked completely
                    string curKey = curX + "." + curY;
                    if (checkedNodes.ContainsKey(curKey))
                    {
                        continue;
                    }

                    // old node
                    if (checkingNodes.ContainsKey(curKey))
                    {
                        Node updatedNode = checkingNodes[curKey];
                        int updatedG = updatedNode.Parent.G + 1;
                        if (updatedG < updatedNode.G)
                        {
                            updatedNode.G = updatedG;
                            updatedNode.F = updatedNode.G + updatedNode.H;
                            updatedNode.Parent = currentSmallestNode.Value;
                        }
                    }
                    // new node
                    else
                    {
                        Node curNode = new Node { X = curX, Y = curY };
                        curNode.Parent = currentSmallestNode.Value;
                        curNode.G = curNode.Parent.G + 1;
                        curNode.H = Math.Abs(curX - toX) + Math.Abs(curY - toY);
                        curNode.F = curNode.G + curNode.H;
                        checkingNodes.Add(curKey, curNode);
                    }
                }
            }
        }

        /// <summary>
        /// Set search order.
        /// </summary>
        public void SetSearchOrder(ESearchOrder order)
        {
            KeyValuePair<int, int> left = new KeyValuePair<int, int>(-1, 0);
            KeyValuePair<int, int> right = new KeyValuePair<int, int>(1, 0);
            KeyValuePair<int, int> up = new KeyValuePair<int, int>(0, -1);
            KeyValuePair<int, int> down = new KeyValuePair<int, int>(0, 1);

            mSearchOder = order;
            switch (order)
            {
                case ESearchOrder.LeftRightUpDown:
                    {
                        mMoveDirections[0] = left;
                        mMoveDirections[1] = right;
                        mMoveDirections[2] = up;
                        mMoveDirections[3] = down;
                    }
                    break;
                case ESearchOrder.LeftRightDownUp:
                    {
                        mMoveDirections[0] = left;
                        mMoveDirections[1] = right;
                        mMoveDirections[2] = down;
                        mMoveDirections[3] = up;
                    }
                    break;
                case ESearchOrder.RightLeftUpDown:
                    {
                        mMoveDirections[0] = right;
                        mMoveDirections[1] = left;
                        mMoveDirections[2] = up;
                        mMoveDirections[3] = down;
                    }
                    break;
                case ESearchOrder.RightLeftDownUp:
                    {
                        mMoveDirections[0] = right;
                        mMoveDirections[1] = left;
                        mMoveDirections[2] = down;
                        mMoveDirections[3] = up;
                    }
                    break;
                case ESearchOrder.UpDownLeftRight:
                    {
                        mMoveDirections[0] = up;
                        mMoveDirections[1] = down;
                        mMoveDirections[2] = left;
                        mMoveDirections[3] = right;
                    }
                    break;
                case ESearchOrder.UpDownRightLeft:
                    {
                        mMoveDirections[0] = up;
                        mMoveDirections[1] = down;
                        mMoveDirections[2] = right;
                        mMoveDirections[3] = left;
                    }
                    break;
                case ESearchOrder.DownUpLeftRight:
                    {
                        mMoveDirections[0] = down;
                        mMoveDirections[1] = up;
                        mMoveDirections[2] = left;
                        mMoveDirections[3] = right;
                    }
                    break;
                case ESearchOrder.DownUpRightLeft:
                    {
                        mMoveDirections[0] = down;
                        mMoveDirections[1] = up;
                        mMoveDirections[2] = right;
                        mMoveDirections[3] = left;
                    }
                    break;
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum ESearchOrder
        {
            LeftRightUpDown,
            LeftRightDownUp,
            RightLeftUpDown,
            RightLeftDownUp,
            UpDownLeftRight,
            UpDownRightLeft,
            DownUpLeftRight,
            DownUpRightLeft
        }

        #endregion

        #region ----- Internal Class -----

        public class Node
        {
            public int X, Y;        // coordinate of this node
            public int G;           // the length of the path from the start node to this node
            public int H;           // the straight-line distance from this node to the end node
            public int F;           // the total distance if taking this route
            public Node Parent;     // the previous node of this node
        }

        #endregion
    }
}
