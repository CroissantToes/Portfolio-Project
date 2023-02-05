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
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    //Mouse hover over tile
    private void OnMouseEnter()
    {
        whiteHL.SetActive(true);

        if (occupant != null)
        {
            if (gameManager.selectedUnit == null)
            {
                occupant.ShowMoveArea();
            }
        }
    }
    private void OnMouseExit()
    {
        whiteHL.SetActive(false);

        if (occupant != null && occupant.hasMoved == false)
        {
            if (gameManager.selectedUnit == null)
            {
                occupant.HideMoveArea();
            }
        }
    }

    //Click on tile
    private void OnMouseUpAsButton()
    {
        if(occupant != null)
        {
            if(occupant.IsHero == true)
            {
                if(gameManager.selectedUnit == occupant)
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
        if(gameManager.selectedUnit != null && gameManager.selectedUnit.IsHero == true)
        {
            foreach (Tile tile in gameManager.selectedUnit.TilesInRange)
            {
                if(tile == this)
                {
                    Hero selectedHero = (Hero)gameManager.selectedUnit;
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
