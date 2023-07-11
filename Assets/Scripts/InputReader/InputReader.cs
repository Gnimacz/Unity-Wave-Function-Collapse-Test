using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Helpers;

public class InputReader : IInputReader<TileBase>
{
    private Tilemap _tilemap;

    public InputReader(Tilemap tilemap)
    {
        _tilemap = tilemap;
    }
    public IValue<TileBase>[][] ReadInput()
    {
        var grid = ReadInputTileMap();

        TileBaseValue[][] tileBaseValueGrid = null;
        if(grid != null){
            //create a jagged array of TileBaseValue
            tileBaseValueGrid = CollectionExtension.CreateJaggedArray<TileBaseValue[][]>(grid.Length, grid[0].Length);
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    tileBaseValueGrid[i][j] = new TileBaseValue(grid[i][j]);
                }
            }
        }
        return tileBaseValueGrid;
    }

    private TileBase[][] ReadInputTileMap()
    {
        InputImageParameters imageParameters = new InputImageParameters(_tilemap);
        return CreateTileBaseGrid(imageParameters);
    }

    private TileBase[][] CreateTileBaseGrid(InputImageParameters imageParameters)
    {
        TileBase[][] grid = null;
        //create a jagged array of TileBase
        grid = CollectionExtension.CreateJaggedArray<TileBase[][]>(imageParameters.Height, imageParameters.Width);
        for (int row = 0; row < imageParameters.Height; row++)
        {
            for (int col = 0; col < imageParameters.Width; col++)
            {
                grid[row][col] = imageParameters.StackOfTiles.Dequeue().Tile;
            }
        }
        return grid;

    }
}
