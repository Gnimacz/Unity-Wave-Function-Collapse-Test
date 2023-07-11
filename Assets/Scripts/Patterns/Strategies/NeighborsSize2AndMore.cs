using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborsSize2AndMore : IFindNeighborStrategy
{
    public Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResults patterFinderResult)
    {
        Dictionary<int, PatternNeighbors> neighborsDictionary = new Dictionary<int, PatternNeighbors>();
        foreach (var item in patterFinderResult.PatternIndexDictionary)
        {
            foreach (var possibleNeighbor in patterFinderResult.PatternIndexDictionary)
            {
                FindNeighborsInAllDirections(neighborsDictionary, item, possibleNeighbor);
            }
        }
        return neighborsDictionary;
    }

    private void FindNeighborsInAllDirections(Dictionary<int, PatternNeighbors> neighborsDictionary, KeyValuePair<int, PatternData> patternDataToCheck, KeyValuePair<int, PatternData> possibleNeighbor)
    {
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            if(patternDataToCheck.Value.CompareGrid(dir, possibleNeighbor.Value))
            {
               if(!neighborsDictionary.ContainsKey(patternDataToCheck.Key))
                {
                    neighborsDictionary.Add(patternDataToCheck.Key, new PatternNeighbors());
                }
                neighborsDictionary[patternDataToCheck.Key].AddPatternToDictionary(dir, possibleNeighbor.Key);
            }
        }
    }
}
