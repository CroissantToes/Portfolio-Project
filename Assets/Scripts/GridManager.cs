using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public Tile[] Tiles { get; private set; }
    public Dictionary<Vector2, Tile> TilesByCoordinates;

    private void Awake()
    {
        Tiles = FindObjectsOfType<Tile>();
        TilesByCoordinates = new Dictionary<Vector2, Tile>();
        foreach(Tile tile in Tiles)
        {
            TilesByCoordinates.Add(tile.coordinates, tile);
        }
    }
}
