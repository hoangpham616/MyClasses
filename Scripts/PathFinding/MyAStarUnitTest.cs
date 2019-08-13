/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyAStarUnitTest (version 1.0)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyAStarUnitTest : MonoBehaviour
    {
        #region ----- Variable -----

        private int[][] mRandomGrid = new int[512][];

        #endregion

        #region ----- MonoBehaviour Implemention -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            Debug.Log("Press key 'Space' create random grid 512x512");
            Debug.Log("Press key '0' to test on random grid");
            Debug.Log("Press key '1-6' to test on fixed grids");

            for (int i = 0; i < mRandomGrid.Length; i++)
            {
                mRandomGrid[i] = new int[mRandomGrid.Length];
            }
            _RandomGrid();
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _RandomGrid();
            }

            if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 S . 1 1 1 1 1
                // . . . . . . . . . . .
                // 1 1 1 1 1 . E 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                // 1 1 1 1 1 . 1 1 1 1 1
                Vector2 startPoint = new Vector2(5, 5);
                Vector2 endPoint = new Vector2(mRandomGrid.Length - 6, mRandomGrid.Length - 6);
                StartCoroutine(_TestAStar(mRandomGrid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.LeftRightUpDown));
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                // 1 1 1 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 1 1 1
                int[][] grid = new int[][] {new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 0, 0, 0, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1}};
                Vector2 startPoint = new Vector2(0, 2);
                Vector2 endPoint = new Vector2(4, 2);
                StartCoroutine(_TestAStar(grid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.LeftRightUpDown));
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                // 1 1 1 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 1 1 1
                int[][] grid = new int[][] {new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 0, 0, 0, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1}};
                Vector2 startPoint = new Vector2(0, 2);
                Vector2 endPoint = new Vector2(4, 2);
                StartCoroutine(_TestAStar(grid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.LeftRightDownUp));
            }

            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                // 1 1 1 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 1 1 1
                int[][] grid = new int[][] {new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 0, 0, 0, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1}};
                Vector2 startPoint = new Vector2(0, 2);
                Vector2 endPoint = new Vector2(4, 2);
                StartCoroutine(_TestAStar(grid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.UpDownLeftRight));
            }

            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                // 1 1 1 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 0 1 1
                // 1 1 1 1 1
                int[][] grid = new int[][] {new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 0, 0, 0, 1},
                                            new int[] {1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1}};
                Vector2 startPoint = new Vector2(0, 2);
                Vector2 endPoint = new Vector2(4, 2);
                StartCoroutine(_TestAStar(grid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.DownUpRightLeft));
            }

            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                // 1 1 1 1 1 1 1 0 1 1
                // 1 1 1 1 0 1 0 1 1 1
                // 1 1 1 S 1 0 1 1 0 E
                // 1 1 0 1 0 1 1 0 1 1
                // 1 1 0 0 1 1 1 0 1 0
                // 1 1 1 1 1 1 1 1 1 1
                int[][] grid = new int[][] {new int[] {1, 1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 0, 0, 1},
                                            new int[] {1, 1, 1, 1, 0, 1},
                                            new int[] {1, 0, 1, 0, 1, 1},
                                            new int[] {1, 1, 0, 1, 1, 1},
                                            new int[] {1, 0, 1, 1, 1, 1},
                                            new int[] {0, 1, 1, 0, 0, 1},
                                            new int[] {1, 1, 0, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 0, 1}};
                Vector2 startPoint = new Vector2(3, 2);
                Vector2 endPoint = new Vector2(9, 2);
                StartCoroutine(_TestAStar(grid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.LeftRightUpDown));
            }

            if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                // 1 1 1 1 1 1 1 0 1 1
                // 1 1 1 1 0 1 0 1 1 1
                // 1 1 1 S 1 0 1 1 0 E
                // 1 1 0 1 0 1 1 0 1 1
                // 1 1 0 0 1 1 1 0 1 0
                // 1 1 1 1 1 1 1 1 1 1
                int[][] grid = new int[][] {new int[] {1, 1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 1, 1},
                                            new int[] {1, 1, 1, 0, 0, 1},
                                            new int[] {1, 1, 1, 1, 0, 1},
                                            new int[] {1, 0, 1, 0, 1, 1},
                                            new int[] {1, 1, 0, 1, 1, 1},
                                            new int[] {1, 0, 1, 1, 1, 1},
                                            new int[] {0, 1, 1, 0, 0, 1},
                                            new int[] {1, 1, 0, 1, 1, 1},
                                            new int[] {1, 1, 1, 1, 0, 1}};
                Vector2 startPoint = new Vector2(3, 2);
                Vector2 endPoint = new Vector2(9, 2);
                StartCoroutine(_TestAStar(grid, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, MyAStar.ESearchOrder.UpDownLeftRight));
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Generate a new grid.
        /// </summary>
        private void _RandomGrid()
        {
            for (int i = 0; i < mRandomGrid.Length; i++)
            {
                int randomStartIndex = UnityEngine.Random.Range(6, 12);
                int randomEndIndex = randomStartIndex + UnityEngine.Random.Range(12, 15);
                for (int j = 0; j < mRandomGrid.Length / 2; j++)
                {
                    int index = j % 20;
                    if (randomStartIndex <= index && index <= randomEndIndex)
                    {
                        mRandomGrid[i][j] = 0;
                    }
                    else
                    {
                        mRandomGrid[i][j] = 1;
                    }
                }

                randomStartIndex = UnityEngine.Random.Range(1, 6);
                randomEndIndex = randomStartIndex + UnityEngine.Random.Range(1, 3);
                for (int j = mRandomGrid.Length / 2; j < mRandomGrid.Length; j++)
                {
                    int index = j % 10;
                    if (randomStartIndex <= index && index <= randomEndIndex)
                    {
                        mRandomGrid[i][j] = 0;
                    }
                    else
                    {
                        mRandomGrid[i][j] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Test AStar algorithm.
        /// </summary>
        private IEnumerator _TestAStar(int[][] grid, int fromX, int fromY, int toX, int toY, MyAStar.ESearchOrder searchOrder)
        {
            int X = grid.GetLength(0);
            int Y = grid[0].Length;

            Debug.Log("---------- AStar Test - BEGIN ----------");
            Debug.Log("grid size: " + X + "x" + Y);
            Debug.Log("search order: " + searchOrder.ToString());
            yield return null;

            DateTime start = DateTime.Now;
            List<Vector2> path = new MyAStar(searchOrder).Search(grid, fromX, fromY, toX, toY);
            DateTime end = DateTime.Now;
            Debug.Log("process in: " + (int)(end - start).TotalMilliseconds + "ms");
            yield return null;

            if (X < 128)
            {
                string debugGrid = string.Empty;
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        if (x == fromX && y == fromY)
                        {
                            debugGrid += "S\t";
                        }
                        else if (x == toX && y == toY)
                        {
                            debugGrid += "E\t";
                        }
                        else if (grid[x][y] > 0)
                        {
                            debugGrid += "O\t";
                        }
                        else
                        {
                            debugGrid += "X\t";
                        }
                    }
                    debugGrid += "\n";
                }
                Debug.Log("graph:\n" + debugGrid);
            }

            if (path != null && X < 128)
            {
                string debugGrid = string.Empty;
                int count = path.Count;
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        if (x == fromX && y == fromY)
                        {
                            debugGrid += "S\t";
                        }
                        else if (x == toX && y == toY)
                        {
                            debugGrid += "E\t";
                        }
                        else if (path.Contains(new Vector2(x, y)))
                        {
                            debugGrid += ".\t";
                        }
                        else if (grid[x][y] > 0)
                        {
                            debugGrid += "O\t";
                        }
                        else
                        {
                            debugGrid += "X\t";
                        }
                    }
                    debugGrid += "\n";
                }
                Debug.Log("path:\n" + debugGrid);

                debugGrid = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (i > 0)
                    {
                        debugGrid += " -> ";
                    }
                    debugGrid += i + ": (" + path[i].x + "," + path[i].y + ")";
                }
                Debug.Log("nodes of path:\n" + debugGrid);
            }

            Debug.Log("---------- AStar Test - END ----------");

            yield return null;
        }

        #endregion
    }
}
