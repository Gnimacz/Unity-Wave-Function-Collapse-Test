using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreSolver
{
    PatternManager patternManager;
    OutputGrid outputGrid;
    CoreHelper coreHelper;
    PropagationHelper propagationHelper;

    public CoreSolver(OutputGrid grid, PatternManager patternManager)
    {
        this.patternManager = patternManager;
        this.outputGrid = grid;
        coreHelper = new CoreHelper(this.patternManager);
        propagationHelper = new PropagationHelper(this.coreHelper, this.outputGrid);
    }

    public void Propagate()
    {
        while(propagationHelper.PairsToPropagate.Count > 0){
            var propagatePair = propagationHelper.PairsToPropagate.Dequeue();
            if(propagationHelper.ShouldProcessPair(propagatePair)){
                ProcessCell(propagatePair);
            }
            if(propagationHelper.CheckForConflicts() || outputGrid.CheckIfGridIsSolved()){
                return;
            }
        }
        if(propagationHelper.CheckForConflicts() || propagationHelper.PairsToPropagate.Count == 0 && propagationHelper.LowEntropySet.Count == 0){ 
            propagationHelper.SetConflictFlag();
        }
    }

    private void ProcessCell(VectorPair propagatePair)
    {
        if(outputGrid.CheckIfCellIsCollapsed(propagatePair.CellToPropogatePosition)){
            propagationHelper.EnqueueUncollapsedNeighbor(propagatePair);
        }
        else{
            PropogateNeighbor(propagatePair);
        }
    }

    private void PropogateNeighbor(VectorPair propagatePair)
    {
        var possibleValuesAtNeighbor = outputGrid.GetPossibleValueForPosition(propagatePair.CellToPropogatePosition);
        int startCount = possibleValuesAtNeighbor.Count;

        RemoveImpossibleNeighbors(propagatePair, possibleValuesAtNeighbor);

        int newPossiblePatternCount = possibleValuesAtNeighbor.Count;
        propagationHelper.AnalyzePropagationResults(propagatePair, startCount, newPossiblePatternCount);
    }

    private void RemoveImpossibleNeighbors(VectorPair propagatePair, HashSet<int> possibleValuesAtNeighbor)
    {
        HashSet<int> possibleIndices = new HashSet<int>();
        foreach (var pattern in outputGrid.GetPossibleValueForPosition(propagatePair.BaseCellPosition))
        {
            var possibleNeighborsForBase = patternManager.GetPossibleNeighborsForPatternInDirection(pattern, propagatePair.DirectionFromBase);
            possibleIndices.UnionWith(possibleNeighborsForBase);
        }

        possibleValuesAtNeighbor.IntersectWith(possibleIndices);
    }

    public Vector2Int GetLowestEntropyCell(){
        if(propagationHelper.LowEntropySet.Count <= 0){
            return outputGrid.GetRandomCell();
        }
        else{
            var lowestEntropyCell = propagationHelper.LowEntropySet.First();
            Vector2Int returnVector = lowestEntropyCell.position;
            propagationHelper.LowEntropySet.Remove(lowestEntropyCell);
            return returnVector;
        }
    }

    public void CollapseCell(Vector2Int cellCoordinates){
        var possibleValue = outputGrid.GetPossibleValueForPosition(cellCoordinates).ToList();

        if(possibleValue.Count == 0 || possibleValue.Count == 1){
            return;
        }
        else{
            int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValue);
            outputGrid.SetPatternAtPosition(cellCoordinates.x, cellCoordinates.y, index);
        }
        if(!coreHelper.CheckCellSolutionForCollision(cellCoordinates, outputGrid)){
            propagationHelper.AddNewPairsToPropagateQueue(cellCoordinates, cellCoordinates);
        }
        else{
            propagationHelper.SetConflictFlag();
        }
    }

    public bool CheckIfSolved(){
        return outputGrid.CheckIfGridIsSolved();
    }

    public bool CheckForConflicts(){
        return propagationHelper.CheckForConflicts();
    }
    
}
