using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Helpers;

public class PatternManager
{
    Dictionary<int, PatternData> patternDataIndexDictionary = new Dictionary<int, PatternData>();
    Dictionary<int, PatternNeighbors> patternNeighborsIndexDictionary = new Dictionary<int, PatternNeighbors>();
    int patternSize = -1;
    IFindNeighborStrategy strategy;

    public PatternManager(int patternSize)
    {
        this.patternSize = patternSize;
    }

    public void processGrid<T>(ValueManager<T> valueManager, bool equalWeights, string strategyName = null)
    {
        NeightborStragetyFactory factory = new NeightborStragetyFactory();
        strategy = factory.CreateInstance(strategyName == null ? patternSize + "" : strategyName);
        CreatePatterns(valueManager, strategy, equalWeights);
    }

    private void CreatePatterns<T>(ValueManager<T> valueManager, IFindNeighborStrategy strategy, bool equalWeights)
    {
        var patterFinderResult = PatternFinder.GetPatternDataFromGrid(valueManager, patternSize, equalWeights);
        patternDataIndexDictionary = patterFinderResult.PatternIndexDictionary;
        GetPatternNeighbors(patterFinderResult, strategy);
    }

    private void GetPatternNeighbors(PatternDataResults patterFinderResult, IFindNeighborStrategy strategy)
    {
        patternNeighborsIndexDictionary = PatternFinder.FindPossibleNeighborsForAllPatterns(strategy, patterFinderResult);

    }

    public PatternData GetPatternDataFromIndex(int patternIndex)
    {
        return patternDataIndexDictionary[patternIndex];
    }
    
    public HashSet<int> GetPossibleNeighborsForPatternInDirection(int patternIndex, Direction direction)
    {
        return patternNeighborsIndexDictionary[patternIndex].GetNeighborsInDirection(direction);
    }

    public float GetPatternFrequency(int index){
        return GetPatternDataFromIndex(index).FrequencyRelative;
    }

    public float GetPatternFrequencyLog2(int index){
        return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
    }

    public int GetNumberOfPatterns(){
        return patternDataIndexDictionary.Count;
    }

    internal int[][] ConvertPatternToValues<T>(int[][] outputValues)
    {
        int patternOutputWidth = outputValues[0].Length;
        int patternOutputHeight = outputValues.Length;
        int valueGridWidth = patternOutputWidth + patternSize - 1;
        int valueGridHeight = patternOutputHeight + patternSize - 1; 
        int[][] valueGrid = CollectionExtension.CreateJaggedArray<int[][]>(valueGridHeight, valueGridWidth);
        for (int row = 0; row < patternOutputHeight; row++)
        {
            for (int col = 0; col < patternOutputWidth; col++)
            {
                Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern; 
                GetPatternValues(patternOutputWidth, patternOutputHeight, valueGrid, row, col, pattern);
            }
        }
        return valueGrid;
    }

    private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valueGrid, int row, int col, Pattern pattern)
    {
        if(row == patternOutputHeight - 1 && col == patternOutputWidth - 1){
            for (int row_1 = 0; row_1 < patternSize; row_1++)
            {
                for (int col_1 = 0; col_1 < patternSize; col_1++)
                {
                    valueGrid[row + row_1][col + col_1] = pattern.GetGridValueAt(col_1, row_1);
                }
            }
        }
        else if(row == patternOutputHeight - 1){
            for (int row_1 = 0; row_1 < patternSize; row_1++)
            {
                valueGrid[row + row_1][col] = pattern.GetGridValueAt(0, row_1);
            }
        }
        else if(col == patternOutputWidth - 1){
            for (int col_1 = 0; col_1 < patternSize; col_1++)
            {
                valueGrid[row][col + col_1] = pattern.GetGridValueAt(col_1, 0);
            }
        }
        else{
            valueGrid[row][col] = pattern.GetGridValueAt(0, 0);
        }
    }
}
