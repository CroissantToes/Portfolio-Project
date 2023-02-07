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

        if (occupant != null && occupant.GetType() != typeof(Barrier))
        {
            if (GameManager.Instance.selectedUnit == null)
            {
                occupant.SetMoveArea();
                GridManager.Instance.ShowMoveArea(occupant);
            }
        }
    }
    private void OnMouseExit()
    {
        whiteHL.SetActive(false);

        if (occupant != null && occupant.hasMoved == false && occupant.GetType() != typeof(Barrier))
        {
            if (GameManager.Instance.selectedUnit == null)
            {
                GridManager.Instance.HideMoveArea(occupant);
            }
        }
    }

    //Click on tile
    private void OnMouseUpAsButton()
    {
        if(occupant != null && occupant.GetType() != typeof(Barrier))
        {
            if(occupant.IsHero == true)
            {
                if(GameManager.Instance.selectedUnit == occupant)
                {
                    occupant.DeselectUnit();
                }
                else
                {
                    occupant.SelectUnit();
                }
            }
        }

        TryMove();
    }

    //If this tile can be moved to be a selected hero, move them here
    private void TryMove()
    {
        if(GameManager.Instance.selectedUnit != null && GameManager.Instance.selectedUnit.IsHero == true)
        {
            foreach (Tile tile in GameManager.Instance.selectedUnit.TilesInMoveRange)
            {
                if(tile == this)
                {
                    Hero selectedHero = (Hero)GameManager.Instance.selectedUnit;
                    selectedHero.MoveToTile(this);
                    break;
                }
            }
        }
    }

    public void ShowBlueHL(bool state)
    {
        blueHL.SetActive(state);
    }
}
