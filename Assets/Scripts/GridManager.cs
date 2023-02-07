using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    #region Singleton
    public static GridManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public Tile[] Tiles { get; private set; }
    public Dictionary<Vector2, Tile> TilesByCoordinates;

    private void Start()
    {
        Tiles = FindObjectsOfType<Tile>();
        TilesByCoordinates = new Dictionary<Vector2, Tile>();
        foreach(Tile tile in Tiles)
        {
            TilesByCoordinates.Add(tile.coordinates, tile);
        }
    }

    public void ShowMoveArea(Unit unit)
    {
        if (GameManager.Instance.state == GameState.playerturn)
        {
            unit.SetMoveArea();
            foreach (Tile tile in unit.TilesInMoveRange)
            {
                tile.ShowBlueHL(true);
            }
        }
    }
    public void HideMoveArea(Unit unit)
    {
        if (GameManager.Instance.state == GameState.playerturn)
        {
            foreach (Tile tile in unit.TilesInMoveRange)
            {
                tile.ShowBlueHL(false);
            }
        }
    }
}
