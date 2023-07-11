using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborsSize1Default : IFindNeighborStrategy
{
    public Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResults patterFinderResult)
    {
        Dictionary<int, PatternNeighbors> neighborsDictionary = new Dictionary<int, PatternNeighbors>();
        FindNeighborsForEachPattern(patterFinderResult, neighborsDictionary);
        return neighborsDictionary;
    }

    private void FindNeighborsForEachPattern(PatternDataResults patterFinderResult, Dictionary<int, PatternNeighbors> neighborsDictionary)
    {
        for (int row = 0; row < patterFinderResult.GetGridLengthY(); row++)
        {
            for (int col = 0; col < patterFinderResult.GetGridLengthX(); col++)
            {
                PatternNeighbors patternNeighbors = PatternFinder.CheckNeighborsInEachDirection(col, row, patterFinderResult);
                PatternFinder.AddNeighborsToDictionary(neighborsDictionary, patterFinderResult.GetPatternIndexAt(col, row), patternNeighbors);
            }
        }

    }
}
