using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileContainer
{
    public Vector2Int Position { get; set; }
    public TileBase Tile { get; set; }

    public TileContainer(Vector2Int position, TileBase tile)
    {
        Position = position;
        Tile = tile;
    }
}
