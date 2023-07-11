using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class Pattern
{
    int index;
    private int[][] grid;

    public string HashIndex { get; set; }
    public int Index { get => index; }

    public Pattern(int[][] grid, int index, string hashCode)
    {
        this.grid = grid;
        this.index = index;
        HashIndex = hashCode;
    }

    public void SetGridValueAt(int x, int y, int value)
    {
        grid[y][x] = value;
    }

    public int GetGridValueAt(int x, int y)
    {
        return grid[y][x];
    }

    public bool CheckValueAtPosition(int x, int y, int value)
    {
        return value.Equals(GetGridValueAt(x, y));
    }

    internal bool CompareToOther(Direction direction, Pattern other)
    {
        int[][] localGrid = GetGridValuesInDirection(direction);
        int[][] otherGrid = other.GetGridValuesInDirection(DirectionHelper.GetOppositeDirection(direction));

        for (int row = 0; row < localGrid.Length; row++)
        {
            for (int column = 0; column < localGrid[0].Length; column++)
            {
                if (localGrid[row][column] != otherGrid[row][column])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private int[][] GetGridValuesInDirection(Direction direction)
    {
        int[][] localGrid;
        switch (direction)
        {
            case Direction.Up:
                localGrid = CollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                CreatePartOfGrid(0, grid.Length, 1, grid.Length, localGrid);
                break;
            case Direction.Down:
                localGrid = CollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, localGrid);
                break;
            case Direction.Left:
                localGrid = CollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, localGrid);
                break;
            case Direction.Right:
                localGrid = CollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                CreatePartOfGrid(1, grid.Length, 0, grid.Length, localGrid);
                break;
            default:
                return grid;
        }
        return localGrid;
    }

    private void CreatePartOfGrid(int xMin, int xMax, int yMin, int yMax, int[][] gridToCompare)
    {
        List<int> tempList = new List<int>();
        for (int row = yMin; row < yMax; row++)
        {
            for (int column = xMin; column < xMax; column++)
            {
                tempList.Add(grid[row][column]);
            }
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            int x = i % gridToCompare.Length;
            int y = i / gridToCompare.Length;
            gridToCompare[x][y] = tempList[i];
        }
    }
}
