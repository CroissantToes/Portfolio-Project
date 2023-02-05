using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    public int Health { 
        get
        {
            return health;
        }
        set
        {
            if(value < 0) health = 0;
            else if(value > maxHealth) health = maxHealth;
            else health = value;
        }
    }
    private int health;
    protected GridManager gridManager;
    protected GameManager gameManager;
    [SerializeField] protected int moveDistance;
    public Tile currentTile;
    public bool canPlay;
    public bool hasMoved;
    public bool IsHero { get; protected set; }
    public Tile[] TilesInRange { get; protected set; }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = FindObjectOfType<GridManager>();
        Health = maxHealth;
    }

    public virtual void SelectUnit()
    {
        Debug.LogWarning("Not implemented in base unit class.", this);
    }
    public virtual void DeselectUnit()
    {
        Debug.LogWarning("Not implemented in base unit class.", this);
    }
    protected virtual void EndTurn()
    {
        Debug.LogWarning("Not implemented in base unit class.", this);
    }

    //Determines what tiles can be moved to
    public void SetMoveArea()
    {
        Predicate<Tile> Pred = (tile) => Math.Abs(tile.coordinates.x - currentTile.coordinates.x)
                                         + Math.Abs(tile.coordinates.y - currentTile.coordinates.y)
                                         <= moveDistance && tile.isObstructed == false;

        TilesInRange = Array.FindAll(gridManager.Tiles, Pred);
    }

    public virtual void ShowMoveArea()
    {
        if (gameManager.state == GameState.playerturn)
        {
            SetMoveArea();
            foreach (Tile tile in TilesInRange)
            {
                tile.ShowBlueHL(true);
            }
        }
    }
    public virtual void HideMoveArea()
    {
        if (gameManager.state == GameState.playerturn)
        {
            foreach (Tile tile in TilesInRange)
            {
                tile.ShowBlueHL(false);
            }
        }
    }
}
