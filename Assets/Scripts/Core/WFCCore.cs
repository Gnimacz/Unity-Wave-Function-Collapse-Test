using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCCore
{
    OutputGrid outputGrid;
    PatternManager patternManager;

    private int maxIterations = 0;

    public WFCCore(int outputWidth, int outputHeight, int maxIterations, PatternManager patternManager)
    {
        this.maxIterations = maxIterations;
        this.patternManager = patternManager;
        outputGrid = new OutputGrid(outputWidth, outputHeight, patternManager.GetNumberOfPatterns());
    }

    public int[][] CreateOutputGrid()
    {
        int iterations = 0;
        while (iterations < maxIterations)
        {
            CoreSolver solver = new CoreSolver(this.outputGrid, this.patternManager);
            int innerIteration = 10500;
            while (!solver.CheckForConflicts() && !solver.CheckIfSolved())
            {
                Vector2Int position = solver.GetLowestEntropyCell();
                solver.Propagate();
                innerIteration--;
                if (innerIteration <= 0)
                {
                    Debug.Log("Inner iteration limit reached");
                    return new int[0][];
                }
                if (solver.CheckForConflicts())
                {
                    Debug.Log("Conflicts found in iteration: " + iterations + " resetting grid");
                    iterations++;
                    outputGrid.ResetAllPossiblePatterns();
                    solver = new CoreSolver(this.outputGrid, this.patternManager);
                }
                else
                {
                    Debug.Log("Solved");
                    outputGrid.PrintResultsToConsole();
                    break;
                }
            }
        }
        if (iterations >= maxIterations)
        {
            Debug.Log("Max iterations reached");
        }
        outputGrid.PrintResultsToConsole();
        return outputGrid.GetSolvedOutputGrid();
    }
}
