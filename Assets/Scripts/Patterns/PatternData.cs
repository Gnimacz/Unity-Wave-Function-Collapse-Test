using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternData
{
    private Pattern _pattern;
    private int frequency;
    private float frequencyRelative;
    private float frequencyRelativeLog2;

    public float FrequencyRelative { get => frequencyRelative; }
    public Pattern Pattern { get => _pattern; }
    public float FrequencyRelativeLog2 { get => frequencyRelativeLog2; }

    public PatternData(Pattern pattern)
    {
        _pattern = pattern;
    }

    public void AddToFrequency()
    {
        frequency++;
    }
    public void CalculateFrequencyRelative(int totalPatterns)
    {
        frequencyRelative = (float)frequency / totalPatterns;
        frequencyRelativeLog2 = Mathf.Log(frequencyRelative, 2);
    }

    public bool CompareGrid(Direction direction, PatternData patternData)
    {
        return _pattern.CompareToOther(direction, patternData.Pattern);
    }
}
