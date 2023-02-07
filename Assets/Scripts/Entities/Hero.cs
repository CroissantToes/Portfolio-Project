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
        if(GameManager.Instance.state == GameState.playerturn && canPlay == true)
        {
            GameManager.Instance.selectedUnit = this;
            HUDManager.Instance.SetSidebar(info, Health, maxHealth, MoveDistance);
            GridManager.Instance.ShowMoveArea(this);
        }
    }

    public override void DeselectUnit()
    {
        GameManager.Instance.selectedUnit = null;
        GridManager.Instance.HideMoveArea(this);
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
        //EndTurn();
    }

    protected override void EndTurn()
    {
        canPlay = false;
        GameManager.Instance.RemoveHeroFromPlay(this);
    }
}
