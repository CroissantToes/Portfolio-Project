using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridMaker : EditorWindow
{
    private GameObject TilePrefab;
    private GameObject GridOrigin;
    private (int Width, int Height) GridDimensions;

    [MenuItem("Tools/Grid Maker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GridMaker));
    }

    private void Awake()
    {
        GridOrigin = GameObject.FindGameObjectWithTag("GridOrigin");
    }

    private void OnGUI()
    {
        TilePrefab = EditorGUILayout.ObjectField("Grid Tile Prefab", TilePrefab, typeof(GameObject), false) as GameObject;
        GridDimensions.Width = EditorGUILayout.IntField("Grid Width", GridDimensions.Width);
        GridDimensions.Height = EditorGUILayout.IntField("Grid Height", GridDimensions.Height);

        if (GUILayout.Button("Create Grid"))
        {
            CreateGrid();
        }
    }

    private void CreateGrid()
    {
        if(GridDimensions.Width <= 0 || GridDimensions.Height <= 0) { Debug.LogError("Grid cannot have negative dimensions.", this); return; }
        if(TilePrefab == null) { Debug.Log("Tile prefab cannot be null.", this); return; }

        for(int y = 0; y < GridDimensions.Height; y++)
        {
            for(int x = 0; x < GridDimensions.Width; x++)
            {
                GameObject NewTile = Instantiate(TilePrefab, new Vector3(x + 0.5f, y + 0.5f, -1f), Quaternion.identity, GridOrigin.transform);
                Tile TileScript = NewTile.GetComponent<Tile>();
                TileScript.coordinates.x = x;
                TileScript.coordinates.y = y;
                NewTile.name = $"Tile ({x},{y})";
            }
        }
    }
}
