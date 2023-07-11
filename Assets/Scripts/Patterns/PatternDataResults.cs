using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class PatternDataResults
{
    private int[][] patternIndices;
    public Dictionary<int, PatternData> PatternIndexDictionary { get; private set; }

    public PatternDataResults(int[][] patternIndices, Dictionary<int, PatternData> patternIndexDictionary)
    {
        this.patternIndices = patternIndices;
        PatternIndexDictionary = patternIndexDictionary;
    }

    public int GetGridLengthX()
    {
        return patternIndices[0].Length;
    }

    public int GetGridLengthY()
    {
        return patternIndices.Length;
    }

    public int GetPatternIndexAt(int x, int y)
    {
        return patternIndices[y][x];
    }

    public int GetNeighborInDirection(int x, int y, Direction direction)
    {
        if (!patternIndices.CheckJaggedArray2IfIndexIsValid(x, y))
        {
            return -1;
        }

        switch (direction)
        {
            case Direction.Up:
                if (patternIndices.CheckJaggedArray2IfIndexIsValid(x, y + 1))
                {
                    return GetPatternIndexAt(x, y + 1);
                }
                return -1;
            case Direction.Down:
                if (patternIndices.CheckJaggedArray2IfIndexIsValid(x, y - 1))
                {
                    return GetPatternIndexAt(x, y - 1);
                }
                return -1;
            case Direction.Left:
                if (patternIndices.CheckJaggedArray2IfIndexIsValid(x - 1, y))
                {
                    return GetPatternIndexAt(x - 1, y);
                }
                return -1;
            case Direction.Right:
                if (patternIndices.CheckJaggedArray2IfIndexIsValid(x + 1, y))
                {
                    return GetPatternIndexAt(x + 1, y);
                }
                return -1;
            default:
                return -1;
                throw new System.Exception("Direction not found");
        }

    }

}
