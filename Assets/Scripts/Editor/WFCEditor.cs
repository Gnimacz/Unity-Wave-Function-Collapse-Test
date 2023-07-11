using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(test))]
public class WFCEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate")) {
            test wfc = (test)target;
            wfc.CreateWFC();
            wfc.CreateTileMap();
        }
    }
}

