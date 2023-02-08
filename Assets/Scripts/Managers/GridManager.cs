using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.CanvasScaler;

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
        if (GameManager.Instance.state == GameState.playerturn && unit.TilesInMoveRange != null)
        {
            if(unit.state == UnitState.ReadyToMove)
            {
                unit.SetMoveArea();
            }
            foreach (Tile tile in unit.TilesInMoveRange)
            {
                tile.ShowBlueHL(true);
            }
        }
    }
    public void HideMoveArea(Unit unit)
    {
        if (GameManager.Instance.state == GameState.playerturn && unit.TilesInMoveRange != null)
        {
            foreach (Tile tile in unit.TilesInMoveRange)
            {
                tile.ShowBlueHL(false);
            }
        }
    }

    public void ShowAttackArea(Hero hero)
    {
        if (GameManager.Instance.state == GameState.playerturn && hero.TilesInAttackRange != null)
        {
            foreach (Tile tile in hero.TilesInAttackRange)
            {
                tile.ShowRedHL(true);
            }
        }
    }
    public void HideAttackArea(Hero hero)
    {
        if (GameManager.Instance.state == GameState.playerturn && hero.TilesInAttackRange != null)
        {
            foreach (Tile tile in hero.TilesInAttackRange)
            {
                tile.ShowRedHL(false);
            }
        }
    }
}
