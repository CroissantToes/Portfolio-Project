using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Unit
{
    public ActionMenu actionMenu;
    public HPBar hpBar;
    [HideInInspector] public bool makingMoveAsAction = false;
    [HideInInspector] public bool autoAttacking = false;
    protected Vector2 targetPosition = new Vector2(-1,-1);

    private void Start()
    {
        hpBar.UpdateBar(Health);
        IsHero = true;
        HealthUpdated += CheckHealth;
    }

    public override void SelectUnit()
    {
        if(GameManager.Instance.state == GameState.playerturn && (state == UnitState.ReadyToMove || state == UnitState.ReadyToAct))
        {
            if(GameManager.Instance.selectedUnit != null)
            {
                GridManager.Instance.HideMoveArea(GameManager.Instance.selectedUnit);
                GameManager.Instance.selectedUnit.DeselectUnit();
            }
            GameManager.Instance.selectedUnit = this;
            HUDManager.Instance.SetSidebar(info, Health, maxHealth, MoveDistance);

            if(state == UnitState.ReadyToMove)
            {
                GridManager.Instance.ShowMoveArea(this);
            }
            actionMenu.SetButtonControls(this, info);
            actionMenu.ShowMenu();
        }
    }

    public override void DeselectUnit()
    {
        if(state != UnitState.MakingMove && state != UnitState.MakingAction)
        {
            GameManager.Instance.selectedUnit = null;
            actionMenu.HideMenu();
        }
    }

    public void MenuCancel()
    {
        GridManager.Instance.HideMoveArea(this);
        if (state == UnitState.ReadyToAct)
        {
            GridManager.Instance.HideAttackArea(this);
        }
    }

    public void ChooseMove()
    {
        state = UnitState.MakingMove;
        actionMenu.HideMenu();
    }

    public void MoveToTile(Tile target)
    {
        GridManager.Instance.HideMoveArea(this);

        //Moves to new tile
        currentTile.isObstructed = false;
        currentTile.occupant = null;
        currentTile = target;
        target.isObstructed = true;
        target.occupant = this;
        gameObject.transform.position = target.transform.position;

        //Sets new unit state
        if(makingMoveAsAction == false)
        {
            state = UnitState.ReadyToAct;
            actionMenu.SetButtonControls(this, info);
            actionMenu.ShowMenu();
        }
        else
        {
            state = UnitState.MakingAction;
        }
    }

    public abstract void Action1();
    public abstract void Action2();

    protected override void CheckHealth(object sender, EventArgs e)
    {
        hpBar.UpdateBar(Health);
        if(Health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void RecieveTargetPosition(Vector2 coordinates)
    {
        targetPosition = coordinates;
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);

        currentTile.isObstructed = false;
        currentTile.occupant = null;
        GameManager.Instance.RemoveHeroFromPool(this);

        Destroy(gameObject);

        yield return null;
    }

    protected override void EndTurn()
    {
        DeselectUnit();
        GridManager.Instance.HideMoveArea(this);
        GridManager.Instance.HideAttackArea(this);
        state = UnitState.Waiting;
        GameManager.Instance.RemoveHeroFromPlay(this);
        TilesInMoveRange = null; 
        TilesInAttackRange = null;
    }
}
