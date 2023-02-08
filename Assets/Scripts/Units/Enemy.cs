using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : Unit
{
    public HPBar hpBar;

    private void Start()
    {
        hpBar.UpdateBar(Health);
        state = UnitState.Waiting;
        IsHero = false;
        HealthUpdated += CheckHealth;
    }

    public override void SelectUnit()
    {
        if (GameManager.Instance.state == GameState.playerturn)
        {
            if (GameManager.Instance.selectedUnit != null)
            {
                GridManager.Instance.HideMoveArea(GameManager.Instance.selectedUnit);
                GameManager.Instance.selectedUnit.DeselectUnit();
            }
            GameManager.Instance.selectedUnit = this;
            HUDManager.Instance.SetSidebar(info, Health, maxHealth, MoveDistance);
            SetMoveArea();
            GridManager.Instance.ShowMoveArea(this);
        }
    }

    public override void DeselectUnit()
    {
        GameManager.Instance.selectedUnit = null;
    }

    public void StartTurn()
    {
        StartCoroutine(PlayTurn());
    }

    private IEnumerator PlayTurn()
    {
        yield return new WaitForSeconds(0.2f);

        SetMoveArea();
        
        LookForMove();

        yield return new WaitForSeconds(0.2f);

        LookForAttackTarget();

        yield return new WaitForSeconds(0.2f);

        EndTurn();

        yield return null;
    }

    public override void SetMoveArea()
    {
        Predicate<Tile> Pred = (tile) => tile.coordinates.x == currentTile.coordinates.x &&
                                         tile.coordinates.y < currentTile.coordinates.y &&
                                         tile.coordinates.y >= currentTile.coordinates.y - MoveDistance;

        TilesInMoveRange = Array.FindAll(GridManager.Instance.Tiles, Pred);
    }

    //Finds tile to move to
    public void LookForMove()
    {
        float start = currentTile.coordinates.y;
        for (float i = start - 1; i > start - (MoveDistance + 1); i--)
        {
            Vector2 vector = new Vector2(currentTile.coordinates.x, i);
            Tile targetTile;
            GridManager.Instance.TilesByCoordinates.TryGetValue(vector, out targetTile);
            if (targetTile.isObstructed == false)
            {
                MoveToTile(targetTile);
            }
            else
            {
                break;
            }
        }
    }

    //MAY USE IN FUTURE
    //VVVVVVVVVVVVVVVVV

    //public virtual Tile LookForMove()
    //{
    //    for(int i = MoveDistance; i > 0; i--)
    //    {
    //        Tile targetTile = null;

    //        foreach(Tile tile in TilesInMoveRange)
    //        {
    //            if(tile.coordinates.x == currentTile.coordinates.x 
    //               && tile.coordinates.y == currentTile.coordinates.y - i)
    //            {
    //                targetTile = tile; 
    //                break;
    //            }
    //        }

    //        if(targetTile != null)
    //        {
    //            return targetTile;
    //        }
    //    }

    //    return currentTile;
    //}

    //Moves to tile
    public void MoveToTile(Tile target)
    {
        this.currentTile.isObstructed = false;
        this.currentTile.occupant = null;
        GridManager.Instance.HideMoveArea(this);
        TilesInMoveRange = null;
        this.currentTile = target;
        target.isObstructed = true;
        target.occupant = this;
        this.gameObject.transform.position = target.transform.position;
    }

    //Looks for target, attacks if one found
    private void LookForAttackTarget()
    {
        Tile targetTile;
        bool targetFound = GridManager.Instance.TilesByCoordinates.TryGetValue(new Vector2(currentTile.coordinates.x, currentTile.coordinates.y - 1f), out targetTile);
        if(targetFound == true)
        {
            if (targetTile.occupant != null && targetTile.occupant.IsHero == true)
            {
                Attack(targetTile.occupant);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    private void Attack(Unit target)
    {
        target.Health -= 2;
    }

    protected override void CheckHealth(object sender, EventArgs e)
    {
        hpBar.UpdateBar(Health);
        if (Health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);

        currentTile.isObstructed = false;
        currentTile.occupant = null;
        GameManager.Instance.RemoveEnemyFromPool(this);

        Destroy(gameObject);

        yield return null;
    }

    protected override void EndTurn()
    {
        state = UnitState.Waiting;
    }
}
