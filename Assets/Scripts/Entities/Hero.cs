using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public bool isDown { get; private set; }

    private void Start()
    {
        IsHero = true;
        isDown = false;
    }

    public override void SelectUnit()
    {
        if(gameManager.state == GameState.playerturn && canPlay == true)
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

    public void MoveToTile(Tile target)
    {
        hasMoved = true;
        currentTile.isObstructed = false;
        currentTile.occupant = null;
        DeselectUnit();
        currentTile = target;
        target.isObstructed = true;
        target.occupant = this;
        gameObject.transform.position = target.transform.position;
        TilesInRange = null;
        EndTurn();
    }

    protected override void EndTurn()
    {
        canPlay = false;
        gameManager.RemoveHeroFromPlay(this);
    }
}
