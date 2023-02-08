using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitPlacer : EditorWindow
{
    private Unit unitToMove;
    private Vector2 newCoordinates = new Vector2(0,0);
    private Tile[] Tiles;
    private Dictionary<Vector2, Tile> TilesByCoordinates;

    [MenuItem("Tools/Unit Placer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UnitPlacer));
    }

    private void OnGUI()
    {
        unitToMove = EditorGUILayout.ObjectField("Unit", unitToMove, typeof(Unit), true) as Unit;
        newCoordinates = EditorGUILayout.Vector2Field("New Tile Coordinates", newCoordinates);

        if (GUILayout.Button("Move Unit"))
        {
            MoveUnit(newCoordinates);
        }
    }

    private void MoveUnit(Vector2 coords)
    {
        InitializeDictionary();
        Tile target = TilesByCoordinates[coords];
        if(unitToMove.currentTile != null)
        {
            unitToMove.currentTile.isObstructed = false;
            unitToMove.currentTile.occupant = null;
        }
        unitToMove.currentTile = target;
        target.isObstructed = true;
        target.occupant = unitToMove;
        unitToMove.gameObject.transform.position = target.transform.position;
    }

    private void InitializeDictionary()
    {
        Tiles = FindObjectsOfType<Tile>();
        TilesByCoordinates = new Dictionary<Vector2, Tile>();
        foreach (Tile tile in Tiles)
        {
            TilesByCoordinates.Add(tile.coordinates, tile);
        }
    }
}
