using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCCore
{
    OutputGrid outputGrid;
    PatternManager patternManager;

    private int maxIterations = 0;

    public WFCCore(int outputWidth, int outputHeight, int maxIterations, PatternManager patternManage)
    {
        this.outputGrid = new OutputGrid(outputWidth, outputHeight, patternManage.GetNumberOfPatterns());
        this.patternManager = patternManage;
        this.maxIterations = maxIterations;
    }

    public int[][] CreateOputputGrid()
    {
        int iteration = 0;
        while (iteration < this.maxIterations)
        {
            CoreSolver solver = new CoreSolver(this.outputGrid, this.patternManager);
            int innerIteration = 100;
            while (!solver.CheckForConflicts() && !solver.CheckIfSolved())
            {
                Vector2Int position = solver.GetLowestEntropyCell();
                solver.CollapseCell(position);
                solver.Propagate();
                innerIteration--;
                if (innerIteration <= 0)
                {
                    Debug.Log("Propagation is taking too long");
                    return new int[0][];
                }
            }
            if (solver.CheckForConflicts())
            {
                Debug.Log("\n Conflict occured. Iteration: " + iteration);
                iteration++;
                outputGrid.ResetAllPossiblePatterns();
                solver = new CoreSolver(this.outputGrid, this.patternManager);
            }
            else
            {
                // if(iteration <= 1) continue;
                Debug.Log("Solved on: " + iteration);
                this.outputGrid.PrintResultsToConsole();
                break;
            }
        }
        if (iteration >= this.maxIterations)
        {
            Debug.Log("Couldn't solve the tilemap");
        }
        return outputGrid.GetSolvedOutputGrid();
    }
}


