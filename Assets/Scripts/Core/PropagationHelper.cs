using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropagationHelper
{
    OutputGrid outputGrid;
    CoreHelper coreHelper;
    bool cellWithNoSolutionFound;
    SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();
    Queue<VectorPair> pairsToPropagate = new Queue<VectorPair>();

    public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet; }
    public Queue<VectorPair> PairsToPropagate { get => pairsToPropagate; }

    public PropagationHelper(CoreHelper coreHelper, OutputGrid outputGrid)
    {
        this.coreHelper = coreHelper;
        this.outputGrid = outputGrid;
    }

    public bool ShouldProcessPair(VectorPair pair)
    {
        return outputGrid.CheckIfValidPosition(pair.CellToPropogatePosition) && pair.IsCheckingPreviousCell() == false;
    }

    public void AnalyzePropagationResults(VectorPair pair, int startCount, int newPossiblePatternCount)
    {
        if (newPossiblePatternCount > 1 && startCount > newPossiblePatternCount)
        {
            AddNewPairsToPropagateQueue(pair.CellToPropogatePosition, pair.BaseCellPosition);
            AddToLowEntropySet(pair.CellToPropogatePosition);
        }
        if (newPossiblePatternCount == 0)
        {
            cellWithNoSolutionFound = true;
        }
        if (newPossiblePatternCount == 1)
        {
            cellWithNoSolutionFound = coreHelper.CheckCellSolutionForCollision(pair.CellToPropogatePosition, outputGrid);
        }
    }

    private void AddToLowEntropySet(Vector2Int cellToPropogatePosition)
    {
        var elementOfLowEntropy = lowEntropySet.Where(x => x.position == cellToPropogatePosition).FirstOrDefault();
        if (elementOfLowEntropy == null && outputGrid.CheckIfCellIsCollapsed(cellToPropogatePosition) == false)
        {
            float entropy = coreHelper.CalculateEntropty(cellToPropogatePosition, outputGrid);
            lowEntropySet.Add(new LowEntropyCell(cellToPropogatePosition, entropy));
        }
        else
        {
            lowEntropySet.Remove(elementOfLowEntropy);
            elementOfLowEntropy.entropy = coreHelper.CalculateEntropty(cellToPropogatePosition, outputGrid);
            lowEntropySet.Add(elementOfLowEntropy);
        }
    }

    public void AddNewPairsToPropagateQueue(Vector2Int cellToPropogatePosition, Vector2Int baseCellPosition)
    {
        var list = coreHelper.Create4DirectionNeighbors(cellToPropogatePosition, baseCellPosition);
        foreach (var item in list)
        {
            pairsToPropagate.Enqueue(item);
        }
    }

    public bool CheckForConflicts()
    {
        return cellWithNoSolutionFound;
    }
    public void SetConflictFlag(){
        cellWithNoSolutionFound = true;
    }

    internal void EnqueueUncollapsedNeighbor(VectorPair propagatePair)
    {
        var uncollapsedNeighbors = coreHelper.CheckIfNeighborsAreCollapsed(propagatePair, outputGrid);
        foreach (var uncollapsed in uncollapsedNeighbors)
        {
            pairsToPropagate.Enqueue(uncollapsed);
        }
    }
}
