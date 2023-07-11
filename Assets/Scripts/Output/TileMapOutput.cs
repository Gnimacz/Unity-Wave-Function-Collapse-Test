using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapOutput : IOutputCreator<Tilemap>
{
    private Tilemap outputImage;
    private ValueManager<TileBase> valueManager;
    public Tilemap OutputImage => outputImage;

    public TileMapOutput(ValueManager<TileBase> valueManager, Tilemap outputImage)
    {
        this.valueManager = valueManager;
        this.outputImage = outputImage;
    }

    public void CreateOutputImage(PatternManager patternManager, int[][] outputValues, int width, int height)
    {
        if(outputValues.Length == 0) return;
        this.outputImage.ClearAllTiles();

        int[][] valueGrid;
        valueGrid = patternManager.ConvertPatternToValues<TileBase>(outputValues);

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++){
                TileBase tile = (TileBase)this.valueManager.GetValue(valueGrid[row][col]).Value;
                this.outputImage.SetTile(new Vector3Int(col, row, 0), tile);
            }
        }
    }
}
