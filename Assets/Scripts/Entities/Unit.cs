using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected event EventHandler HealthUpdated;

    [SerializeField] protected int maxHealth;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value < 0) health = 0;
            else if (value > maxHealth) health = maxHealth;
            else health = value;
            HealthUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    [SerializeField] private int health;
    public int MoveDistance { get => _moveDistance; private set => _moveDistance = value; }
    [SerializeField] private int _moveDistance;
    public Tile currentTile;
    public bool canPlay;
    public bool hasMoved;
    public bool hasAttacked;
    public bool IsHero { get; protected set; }
    [SerializeField] protected UnitInfo info;
    public Tile[] TilesInMoveRange { get; protected set; }

    private void Start()
    {
        Health = maxHealth;
        SetMoveArea();
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

    //Determines what tiles can be moved to by selected unit
    public void SetMoveArea()
    {
        Predicate<Tile> Pred = (tile) => Math.Abs(tile.coordinates.x - currentTile.coordinates.x)
                                         + Math.Abs(tile.coordinates.y - currentTile.coordinates.y)
                                         <= MoveDistance && tile.isObstructed == false;

        TilesInMoveRange = Array.FindAll(GridManager.Instance.Tiles, Pred);
    }
}
