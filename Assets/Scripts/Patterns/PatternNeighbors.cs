using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternNeighbors
{
    public Dictionary<Direction, HashSet<int>> directionNeighborPatternDictionary = new Dictionary<Direction, HashSet<int>>();

    public void AddPatternToDictionary(Direction direction, int patternIndex)
    {
        if (directionNeighborPatternDictionary.ContainsKey(direction))
        {
            directionNeighborPatternDictionary[direction].Add(patternIndex);
        }
        else
        {
            directionNeighborPatternDictionary.Add(direction, new HashSet<int>() { patternIndex });
        }
    }
    internal HashSet<int> GetNeighborsInDirection(Direction direction)
    {
        if (directionNeighborPatternDictionary.ContainsKey(direction))
        {
            return directionNeighborPatternDictionary[direction];
        }
        return new HashSet<int>();
    }

    public void AddNeighbor(PatternNeighbors neighbors)
    {
        foreach (var item in neighbors.directionNeighborPatternDictionary)
        {
            if (!directionNeighborPatternDictionary.ContainsKey(item.Key))
            {
                directionNeighborPatternDictionary.Add(item.Key, new HashSet<int>());
            }
            else
            {
                directionNeighborPatternDictionary[item.Key].UnionWith(item.Value);
            }
            
        }
    }
}
