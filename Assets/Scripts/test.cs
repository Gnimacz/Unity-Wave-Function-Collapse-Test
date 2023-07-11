using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text;

public class test : MonoBehaviour
{
    public Tilemap input;
    private void Start() {
        InputReader inputReader = new InputReader(input);
        var grid = inputReader.ReadInput();
        ValueManager<TileBase> valueManager = new ValueManager<TileBase>(grid);
        PatternManager patternManager = new PatternManager(2);
        patternManager.processGrid(valueManager, false);
        WFCCore wfccore = new WFCCore(6, 6, 500, patternManager);
        var result = wfccore.CreateOputputGrid();






        // for (int i = 0; i < grid.Length; i++)
        // {
        //     for (int j = 0; j < grid[0].Length; j++)
        //     {
        //         Debug.Log("Row: " + i + " Col: " + j + " tile name: " + grid[i][j].Value);
        //     }
        // }
        StringBuilder sb = new StringBuilder();
        List<string> list = new List<string>();
        for (int i = 0; i < grid.Length; i++)
        {
            sb = new StringBuilder();
            for (int j = 0; j < grid[0].Length; j++)
            {
                sb.Append(valueManager.GetGridValuesIncludingOffset(j, i));
                sb.Append(" ");
            }
            list.Add(sb.ToString());
        }
        list.Reverse();
        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }
}
