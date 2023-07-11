using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public static class PatternFinder
{
    internal static Dictionary<int, PatternNeighbors> FindPossibleNeighborsForAllPatterns(IFindNeighborStrategy strategy, PatternDataResults patterFinderResult)
    {
        return strategy.FindNeighbors(patterFinderResult);
    }

    internal static PatternDataResults GetPatternDataFromGrid<T>(ValueManager<T> valueManager, int patternSize, bool equalWeights)
    {
        Dictionary<string, PatternData> patternHashcodeDictionary = new Dictionary<string, PatternData>();
        Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();

        Vector2 gridSize = valueManager.GetGridSize();
        int patternGridSizeX = 0, patternGridSizeY = 0;
        int rowMin = -1, rowMax = -1, colMin = -1, colMax = -1;
        if (patternSize < 3)
        {
            patternGridSizeX = (int)gridSize.x + 3 - patternSize;
            patternGridSizeY = (int)gridSize.y + 3 - patternSize;
            rowMax = (int)gridSize.y - 1;
            colMax = (int)gridSize.x - 1;
        }
        else
        {
            patternGridSizeX = (int)gridSize.x + patternSize - 1;
            patternGridSizeY = (int)gridSize.y + patternSize - 1;
            rowMin = 1 - patternSize;
            colMin = 1 - patternSize;
            rowMax = (int)gridSize.y - 1;
            colMax = (int)gridSize.x - 1;
        }

        int[][] patternIndices = CollectionExtension.CreateJaggedArray<int[][]>(patternGridSizeY, patternGridSizeX);
        int totalFrequency = 0, patternIndex = 0;

        for (int row = rowMin; row < rowMax; row++)
        {
            for (int col = colMin; col < colMax; col++)
            {
                int[][] gridValues = valueManager.GetPatternValuesFromGridAt(col, row, patternSize);
                string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                if (!patternHashcodeDictionary.ContainsKey(hashValue))
                {
                    Pattern pattern = new Pattern(gridValues, patternIndex, hashValue);
                    patternIndex++;
                    AddNewPattern(patternHashcodeDictionary, patternIndexDictionary, hashValue, pattern);
                }
                else
                {
                    if (!equalWeights)
                    {
                        patternIndexDictionary[patternHashcodeDictionary[hashValue].Pattern.Index].AddToFrequency();
                    }
                }
                totalFrequency++;
                if (patternSize < 3)
                {
                    patternIndices[row + 1][col + 1] = patternHashcodeDictionary[hashValue].Pattern.Index;
                }
                else
                {
                    patternIndices[row + patternSize - 1][col + patternSize - 1] = patternHashcodeDictionary[hashValue].Pattern.Index;
                }
            }

        }

        CalculateRelativeFrequency(patternIndexDictionary, totalFrequency);
        return new PatternDataResults(patternIndices, patternIndexDictionary);

    }

    private static void CalculateRelativeFrequency(Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency)
    {
        foreach (var item in patternIndexDictionary.Values)
        {
            item.CalculateFrequencyRelative(totalFrequency);
        }
    }

    private static void AddNewPattern(Dictionary<string, PatternData> patternHashcodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern pattern)
    {
        PatternData patternData = new PatternData(pattern);
        patternHashcodeDictionary.Add(hashValue, patternData);
        patternIndexDictionary.Add(pattern.Index, patternData);
    }

    public static PatternNeighbors CheckNeighborsInEachDirection(int x, int y, PatternDataResults patternDataResults)
    {
        PatternNeighbors patternNeighbors = new PatternNeighbors();
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            int neighborIndex = patternDataResults.GetNeighborInDirection(x, y, dir);
            if (neighborIndex != -1)
            {
                patternNeighbors.AddPatternToDictionary(dir, neighborIndex);
            }
        }
        return patternNeighbors;
    }

    public static void AddNeighborsToDictionary(Dictionary<int, PatternNeighbors> dictionary, int patternIndex, PatternNeighbors patternNeighbors)
    {
        if (dictionary.ContainsKey(patternIndex))
        {
            dictionary[patternIndex].AddNeighbor(patternNeighbors);
        }
        else
        {
            dictionary.Add(patternIndex, patternNeighbors);
        }
    }
}
