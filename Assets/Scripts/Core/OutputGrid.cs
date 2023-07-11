using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using UnityEngine;

public class OutputGrid
{
    Dictionary<int, HashSet<int>> indexPossiblePatternDictionary = new Dictionary<int, HashSet<int>>();
    public int width { get; private set; }
    public int height { get; private set; }
    private int maxPatternNumber = 0;

    public OutputGrid(int width, int height, int patternAmount)
    {
        this.width = width;
        this.height = height;
        maxPatternNumber = patternAmount;
        ResetAllPossiblePatterns();
    }

    public void ResetAllPossiblePatterns()
    {
        HashSet<int> allPossiblePatterns = new HashSet<int>();
        allPossiblePatterns.UnionWith(Enumerable.Range(0, maxPatternNumber).ToList());

        indexPossiblePatternDictionary.Clear();
        for (int i = 0; i < width * height; i++)
        {
            indexPossiblePatternDictionary.Add(i, new HashSet<int>(allPossiblePatterns));
        }
    }

    public bool CheckCellExists(Vector2Int position)
    {
        int index = GetIndexFromPosition(position);
        return indexPossiblePatternDictionary.ContainsKey(index);
    }

    private int GetIndexFromPosition(Vector2Int position)
    {
        return position.x + width * position.y;
    }

    public bool CheckIfCellIsCollapsed(Vector2Int position)
    {
        return GetPossibleValueForPosition(position).Count <= 1;
    }

    public HashSet<int> GetPossibleValueForPosition(Vector2Int position)
    {
        int index = GetIndexFromPosition(position);
        if (indexPossiblePatternDictionary.ContainsKey(index))
        {
            return indexPossiblePatternDictionary[index];
        }
        else return new HashSet<int>();
    }

    public bool CheckIfGridIsSolved()
    {
        return !indexPossiblePatternDictionary.Any(x => x.Value.Count > 1);
    }

    internal bool CheckIfValidPosition(Vector2Int position)
    {
        return CollectionExtension.ValidateCoordinates(position.x, position.y, width, height);
    }

    public Vector2Int GetRandomCell()
    {
        int index = UnityEngine.Random.Range(0, width * height);
        return GetPositionFromIndex(index);
    }

    private Vector2Int GetPositionFromIndex(int index)
    {
        Vector2Int position = Vector2Int.zero;
        position.x = index % width;
        position.y = index / width;
        return position;
    }

    public void SetPatternAtPosition(int x, int y, int patternIndex)
    {
        int index = GetIndexFromPosition(new Vector2Int(x, y));
        indexPossiblePatternDictionary[index] = new HashSet<int>() { patternIndex };
    }

    public int[][] GetSolvedOutputGrid()
    {
        int[][] outputGrid = CollectionExtension.CreateJaggedArray<int[][]>(width, height);
        if (!CheckIfGridIsSolved()) return CollectionExtension.CreateJaggedArray<int[][]>(0, 0);
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                int index = GetIndexFromPosition(new Vector2Int(column, row));
                outputGrid[row][column] = indexPossiblePatternDictionary[index].First();
            }
        }
        return outputGrid;
    }

    internal void PrintResultsToConsole()
    {
        StringBuilder sb = new StringBuilder();
        List<string> list = new List<string>();
        for (int i = 0; i < this.height; i++)
        {
            sb = new StringBuilder();
            for (int j = 0; j < this.width; j++)
            {
                var result = GetPossibleValueForPosition(new Vector2Int(j, i));
                if(result.Count == 1)
                {
                    sb.Append(result.First() + "");
                }
                else
                {
                    string newString = "";
                    foreach (var item in result)
                    {
                        newString += item + ",";
                    }
                    sb.Append(newString);
                }
            }
            list.Add(sb.ToString());
        }
        list.Reverse();
        foreach (var item in list)
        {
            Debug.Log(item);
        }
        Debug.Log("--------------------");
    }
}
