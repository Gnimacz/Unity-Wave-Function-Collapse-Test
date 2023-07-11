using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPair
{
    public Vector2Int BaseCellPosition { get; set; }
    public Vector2Int CellToPropogatePosition { get; set; }
    public Vector2Int PreviousCellPosition { get; set; }
    public Direction DirectionFromBase { get => directionFromBase; set => directionFromBase = value; }

    private Direction directionFromBase;


    public VectorPair(Vector2Int baseCellPosition, Vector2Int cellToPropogatePosition, Vector2Int previousCellPosition, Direction directionFromBase)
    {
        BaseCellPosition = baseCellPosition;
        CellToPropogatePosition = cellToPropogatePosition;
        PreviousCellPosition = previousCellPosition;
        DirectionFromBase = directionFromBase;
    }

    public bool IsCheckingPreviousCell(){
        return PreviousCellPosition == CellToPropogatePosition;
    }

}
