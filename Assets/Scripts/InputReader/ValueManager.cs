using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System.Linq;

public class ValueManager<T>
{
    int[][] grid;
    Dictionary<int, IValue<T>> valueDictionary = new Dictionary<int, IValue<T>>();
    int index = 0;

    public ValueManager(IValue<T>[][] valueGrid)
    {
        CreateGridOfIndices(valueGrid);
    }

    private void CreateGridOfIndices(IValue<T>[][] valueGrid)
    {
        grid = CollectionExtension.CreateJaggedArray<int[][]>(valueGrid.Length, valueGrid[0].Length);
        for (int i = 0; i < valueGrid.Length; i++)
        {
            for (int j = 0; j < valueGrid[0].Length; j++)
            {
                SetIndexToGridPosition(valueGrid, i, j);
            }
        }
    }

    private void SetIndexToGridPosition(IValue<T>[][] valueGrid, int i, int j)
    {
        if(valueDictionary.ContainsValue(valueGrid[i][j]))
        {
            grid[i][j] = valueDictionary.FirstOrDefault(x => x.Value.Equals(valueGrid[i][j])).Key;
        }
        else
        {
            grid[i][j] = index;
            valueDictionary.Add(grid[i][j], valueGrid[i][j]);
            index++;
        }
    }

    public int GetGridValue(int x, int y)
    {
        if (grid.CheckJaggedArray2IfIndexIsValid(x, y))
        {
            return grid[y][x];
        }
        throw new Exception("Index out of bounds");
    }

    public IValue<T> GetValue(int index)
    {
        if (valueDictionary.ContainsKey(index))
        {
            return valueDictionary[index];
        }
        throw new Exception("No Index found in dictionary");
    }

    public int GetGridValuesIncludingOffset(int x, int y){
        int yMax = grid.Length;
        int xMax = grid[0].Length;
        if(x < 0 && y < 0){
            return GetGridValue(xMax + x, yMax + y);
        }
        if(x < 0 && y >= yMax){
            return GetGridValue(xMax + x, y - yMax);
        }
        if(x >= xMax && y < 0){
            return GetGridValue(x - xMax, yMax + y);
        }
        if(x < 0){
            return GetGridValue(xMax + x, y);
        }
        if(x >= xMax){
            return GetGridValue(x - xMax, y);
        }
        if(y < 0){
            return GetGridValue(x, yMax + y);
        }
        if(y >= yMax){
            return GetGridValue(x, y - yMax);
        }
        
        // if(y < 0 && xMax >= 0){
        //     return GetGridValue(x - xMax, yMax + y);
        // }
        // if(x >= xMax && y >= yMax){
        //     return GetGridValue(x - xMax, y - yMax);
        // }
        // if(x < 0){
        //     return GetGridValue(xMax + x, y);
        // }
        // if(x >= xMax){
        //     return GetGridValue(x - xMax, y);
        // }
        // if(y < 0){
        //     return GetGridValue(x, yMax + y);
        // }
        // if(y >= yMax){
        //     return GetGridValue(x, y - yMax);
        // }
        return GetGridValue(x, y);
    }

    public int[][] GetPatternValuesFromGridAt(int x, int y,  int patternSize){
        int[][] arrayToReturn = CollectionExtension.CreateJaggedArray<int[][]>(patternSize, patternSize);
        for (int i = 0; i < patternSize; i++)
        {
            for (int j = 0; j < patternSize; j++)
            {
                arrayToReturn[i][j] = GetGridValuesIncludingOffset(x + j, y + i);
            }
        }
        return arrayToReturn;
    }

    internal Vector2 GetGridSize()
    {
        if(grid == null){
            return new Vector2(0, 0);
        }
        return new Vector2(grid[0].Length, grid.Length);
    }
}
