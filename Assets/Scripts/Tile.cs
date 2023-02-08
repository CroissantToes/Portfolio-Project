using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 coordinates;
    [SerializeField] private GameObject whiteHL;
    [SerializeField] private GameObject blueHL;
    [SerializeField] private GameObject redHL;
    public Unit occupant;
    public bool isObstructed;

    //Mouse hover over tile
    private void OnMouseEnter()
    {
        whiteHL.SetActive(true);

        if (occupant != null && 
           (occupant.state == UnitState.ReadyToMove || 
           (occupant.GetType() == typeof(Enemy) && occupant.state == UnitState.Waiting)) &&
            occupant.GetType() != typeof(Barrier))
        {
            if (GameManager.Instance.selectedUnit == null)
            {
                GridManager.Instance.ShowMoveArea(occupant);
            }
        }
    }
    private void OnMouseExit()
    {
        whiteHL.SetActive(false);

        if (occupant != null &&
            GameManager.Instance.selectedUnit != occupant &&
           (occupant.state == UnitState.ReadyToMove || 
           (occupant.GetType() == typeof(Enemy) && occupant.state == UnitState.Waiting)) &&
            occupant.GetType() != typeof(Barrier))
        {
            if(GameManager.Instance.selectedUnit == null)
            {
                GridManager.Instance.HideMoveArea(occupant);
            }
            if(GameManager.Instance.selectedUnit == occupant)
            {
                GridManager.Instance.ShowMoveArea(occupant);
            }
        }
    }

    //Click on tile
    private void OnMouseUpAsButton()
    {
        if(occupant != null && occupant.GetType() != typeof(Barrier) && GameManager.Instance.state == GameState.playerturn)
        {
            if(occupant.IsHero == true)
            {
                if(GameManager.Instance.selectedUnit == occupant && occupant.state != UnitState.MakingMove && occupant.state != UnitState.MakingAction)
                {
                    occupant.DeselectUnit();
                }
                else
                {
                    occupant.SelectUnit();
                }
            }
            else if(occupant.IsHero == false)
            {
                if(GameManager.Instance.selectedUnit == null || GameManager.Instance.selectedUnit.GetType() == typeof(Enemy) || (GameManager.Instance.selectedUnit.GetType() == typeof(Hero) && (GameManager.Instance.selectedUnit.state == UnitState.ReadyToMove ||
                   GameManager.Instance.selectedUnit.state == UnitState.ReadyToAct)))
                {
                    if(GameManager.Instance.selectedUnit != occupant)
                    {
                        occupant.SelectUnit();
                    }
                    else
                    {
                        occupant.DeselectUnit();
                    }
                }
            }
        }

        TryMove();
        TryAttack();
    }

    //If this tile can be moved to be a selected hero, move them here
    private void TryMove()
    {
        if(GameManager.Instance.selectedUnit != null && 
           GameManager.Instance.selectedUnit.IsHero == true && 
           GameManager.Instance.selectedUnit.state == UnitState.MakingMove)
        {
            foreach (Tile tile in GameManager.Instance.selectedUnit.TilesInMoveRange)
            {
                if(tile == this)
                {
                    Hero selectedHero = (Hero)GameManager.Instance.selectedUnit;
                    selectedHero.MoveToTile(this);
                    GridManager.Instance.HideMoveArea(selectedHero);
                    break;
                }
            }
        }
    }

    private void TryAttack()
    {
        Hero selectedHero = null;
        if (GameManager.Instance.selectedUnit != null &&
           GameManager.Instance.selectedUnit.IsHero == true)
        {
            selectedHero = (Hero)GameManager.Instance.selectedUnit;
        }

        if(selectedHero != null &&
            selectedHero.state == UnitState.MakingAction &&
            selectedHero.makingMoveAsAction == false &&
            selectedHero.autoAttacking == false)
        {
            foreach (Tile tile in GameManager.Instance.selectedUnit.TilesInAttackRange)
            {
                if (tile == this)
                {
                    selectedHero.RecieveTargetPosition(coordinates);
                    break;
                }
            }
        }
    }

    public void ShowBlueHL(bool state)
    {
        blueHL.SetActive(state);
    }

    public void ShowRedHL(bool state)
    {
        redHL.SetActive(state);
    }
}
