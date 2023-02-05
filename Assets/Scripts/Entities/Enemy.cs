using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : Unit
{
    private void Start()
    {
        IsHero = false;
    }

    public override void SelectUnit()
    {
        if (gameManager.state == GameState.playerturn)
        {
            gameManager.selectedUnit = this;
            ShowMoveArea();
        }
    }

    public override void DeselectUnit()
    {
        gameManager.selectedUnit = null;
        HideMoveArea();
    }

    public void StartTurn()
    {
        StartCoroutine(PlayTurn());
    }

    private IEnumerator PlayTurn()
    {
        yield return new WaitForSeconds(1f);

        MoveToTile(LookForMove());

        yield return new WaitForSeconds(1f);

        LookForAttackTarget();

        yield return new WaitForSeconds(1f);

        EndTurn();

        yield return null;
    }

    //Finds tile to move to
    public virtual Tile LookForMove()
    {
        for(int i = moveDistance; i > 0; i--)
        {
            Tile targetTile = null;

            foreach(Tile tile in TilesInRange)
            {
                if(tile.coordinates.x == currentTile.coordinates.x 
                   && tile.coordinates.y == currentTile.coordinates.y - i 
                   && tile.isObstructed == false)
                {
                    targetTile = tile; 
                    break;
                }
            }

            if(targetTile != null)
            {
                return targetTile;
            }
        }

        return currentTile;
    }

    //Moves to tile
    public void MoveToTile(Tile target)
    {
        this.currentTile.isObstructed = false;
        this.currentTile.occupant = null;
        HideMoveArea();
        this.currentTile = target;
        target.isObstructed = true;
        target.occupant = this;
        this.gameObject.transform.position = target.transform.position;
        TilesInRange = null;
    }

    //Looks for target, attacks if one found
    private void LookForAttackTarget()
    {
        Tile targetTile;
        bool targetFound = gridManager.TilesByCoordinates.TryGetValue(new Vector2(currentTile.coordinates.x, currentTile.coordinates.y - 1f), out targetTile);
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
        Debug.Log(target.Health);
        target.Health -= 2;
        Debug.Log(target.Health);
    }

    protected override void EndTurn()
    {
        canPlay = false;
        //gameManager.RemoveEnemyFromPlay(this);
    }
}
