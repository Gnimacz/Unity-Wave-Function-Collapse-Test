using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreHelper
{
    float totalFrequency = 0;
    float totalFrequencyLog = 0;
    PatternManager patternManager;

    public CoreHelper(PatternManager patternManager)
    {
        this.patternManager = patternManager;

        for (int i = 0; i < patternManager.GetNumberOfPatterns(); i++)
        {
            totalFrequency += patternManager.GetPatternFrequency(i);
        }
        totalFrequencyLog = Mathf.Log(totalFrequency, 2);
    }

    public int SelectSolutionPatternFromFrequency(List<int> possibleValues)
    {
        List<float> valueFrequenciesFractions = GetListOfWeightsFromFrequencies(possibleValues);
        float randomValue = UnityEngine.Random.Range(0f, valueFrequenciesFractions.Sum());
        float sum = 0;
        int index = 0;
        foreach (var item in valueFrequenciesFractions)
        {
            sum += item;
            if (randomValue <= sum)
            {
                return index;
            }
            index++;
        }
        return index - 1;
    }

    private List<float> GetListOfWeightsFromFrequencies(List<int> possibleValues)
    {
        var valueFrequencies = possibleValues.Aggregate(new List<float>(), (acc, value) =>
        {
            acc.Add(patternManager.GetPatternFrequency(value));
            return acc;
        },
        acc => acc).ToList();
        return valueFrequencies;
    }

    public List<VectorPair> Create4DirectionNeighbors(Vector2Int cellCoordinates, Vector2Int previousCell)
    {
        List<VectorPair> neighbors = new List<VectorPair>()
        {
            new VectorPair(cellCoordinates, cellCoordinates + Vector2Int.right, previousCell, Direction.Right),
            new VectorPair(cellCoordinates, cellCoordinates + Vector2Int.left, previousCell, Direction.Left),
            new VectorPair(cellCoordinates, cellCoordinates + Vector2Int.up, previousCell, Direction.Up),
            new VectorPair(cellCoordinates, cellCoordinates + Vector2Int.down, previousCell, Direction.Down)
        };
        return neighbors;
    }

    public List<VectorPair> Create4DirectionNeighbors(Vector2Int cellCoordinates){
        return Create4DirectionNeighbors(cellCoordinates, cellCoordinates);
    }
    public float CalculateEntropty(Vector2Int position, OutputGrid outputGrid)
    {
        float sum = 0;
        foreach (var possibleIndex in outputGrid.GetPossibleValueForPosition(position))
        {
            sum += patternManager.GetPatternFrequencyLog2(possibleIndex);
        }
        return totalFrequencyLog - (sum / totalFrequency);
    }

    public List<VectorPair> CheckIfNeighborsAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
    {
        return Create4DirectionNeighbors(pairToCheck.CellToPropogatePosition, pairToCheck.BaseCellPosition).Where(pair => outputGrid.CheckIfValidPosition(pair.CellToPropogatePosition) && outputGrid.CheckIfCellIsCollapsed(pair.CellToPropogatePosition) == false).ToList();
    }
    public bool CheckCellSolutionForCollision(Vector2Int cellCoordinates, OutputGrid outputGrid){
        foreach (var neighborIndex in Create4DirectionNeighbors(cellCoordinates))
        {
            if(!outputGrid.CheckIfValidPosition(neighborIndex.CellToPropogatePosition)){
                continue;
            }
            HashSet<int> possibleNeighbors = new HashSet<int>();
            foreach (var patternIndexAtNeighbor in outputGrid.GetPossibleValueForPosition(neighborIndex.CellToPropogatePosition))
            {
                var possibleNeighborsForPattern = patternManager.GetPossibleNeighborsForPatternInDirection(patternIndexAtNeighbor, DirectionHelper.GetOppositeDirection(neighborIndex.DirectionFromBase));
                possibleNeighbors.UnionWith(possibleNeighborsForPattern);
            }
            if(!possibleNeighbors.Contains(outputGrid.GetPossibleValueForPosition(cellCoordinates).First())){
                return true;
            }
        }
        return false;
    }
}
