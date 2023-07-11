using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
}
