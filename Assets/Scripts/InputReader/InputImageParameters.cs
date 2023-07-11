using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class InputImageParameters
{
    Vector2Int? _bottomRight = null;
    Vector2Int? _topLeft = null;

    BoundsInt _bounds;
    TileBase[] inputTilemapTilesArray;
    Queue<TileContainer> stackOfTiles = new Queue<TileContainer>();
    private int _width = 0, _height = 0;
    private Tilemap _tilemap;
    public Queue<TileContainer> StackOfTiles { get => stackOfTiles; set => stackOfTiles = value; }
    public int Height { get => _height; }
    public int Width { get => _width; }


    public InputImageParameters(Tilemap tilemap)
    {
        _tilemap = tilemap;
        _bounds = tilemap.cellBounds;
        inputTilemapTilesArray = tilemap.GetTilesBlock(_bounds);
        ExtractNonEmptyTiles();
        VerifyInputTiles();
    }

    private void VerifyInputTiles()
    {
        if (_bottomRight == null || _topLeft == null)
        {
            throw new Exception("Input image is empty");
        }
        int minX = _bottomRight.Value.x;
        int minY = _bottomRight.Value.y;
        int maxX = _topLeft.Value.x;
        int maxY = _topLeft.Value.y;
        _width = Math.Abs(maxX - minX) + 1;
        _height = Math.Abs(maxY - minY) + 1;

        int expectedSize = _width * _height;
        if (expectedSize != stackOfTiles.Count)
        {
            throw new Exception("Input image has empty fields!");
        }
        if (stackOfTiles.Any(tile => tile.Position.x < minX || tile.Position.x > maxX || tile.Position.y < minY || tile.Position.y > maxY))
        {
            throw new Exception("Input image should be a rectangle!");
        }
    }

    private void ExtractNonEmptyTiles()
    {
        for (int row = 0; row < _bounds.size.y; row++)
        {
            for (int col = 0; col < _bounds.size.x; col++)
            {
                int index = col + (row * _bounds.size.x);
                TileBase tile = inputTilemapTilesArray[index];
                if (_bottomRight == null && tile != null)
                {
                    _bottomRight = new Vector2Int(col, row);
                }
                if (tile != null)
                {
                    stackOfTiles.Enqueue(new TileContainer(new Vector2Int(col, row), tile));
                    _topLeft = new Vector2Int(col, row);
                }
            }
        }
    }
}
