using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text;
using UnityEditor;

public class test : MonoBehaviour
{
    public Tilemap inputTileMap;
    public Tilemap outputTileMap;
    public int patternSize = 2;
    public int maxInteration = 500;
    public int outputWidth = 5;
    public int outputHeight = 5;
    public bool equalWeights = false;

    ValueManager<TileBase> valueManager;
    WFCCore wfccore;
    PatternManager patternManager;
    TileMapOutput tileMapOutput;

    private void Start()
    {
        CreateWFC();
        CreateTileMap();
        
    }

    public void CreateWFC()
    {
        Debug.Log("Creating WFC");
        InputReader inputReader = new InputReader(inputTileMap);
        var grid = inputReader.ReadInput();
        valueManager = new ValueManager<TileBase>(grid);
        patternManager = new PatternManager(patternSize);
        patternManager.processGrid(valueManager, equalWeights);
        wfccore = new WFCCore(outputWidth, outputHeight, maxInteration, patternManager);
    }

    public void CreateTileMap()
    {
        tileMapOutput = new TileMapOutput(valueManager, outputTileMap);
        var result = wfccore.CreateOputputGrid();
        tileMapOutput.CreateOutputImage(patternManager, result, outputWidth, outputHeight);
    }

    public void SaveTileMap(){
        var path = "Assets/Output/TileMapOutput.asset";
        AssetDatabase.CreateAsset(outputTileMap, path);
    }
}
