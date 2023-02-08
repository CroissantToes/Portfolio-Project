using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Archer : Hero
{
    [Header("Attack Info")]
    [SerializeField] int LongshotDamage = 2;
    [SerializeField] int BullseyeDamage = 4;

    public override void Action1()
    {
        actionMenu.HideMenu();
        StartCoroutine(Longshot());
    }

    private IEnumerator Longshot()
    {
        bool enemyDown = false;
        state = UnitState.MakingAction;

        //Selecting Target

        Predicate<Tile> PredAttack = (tile) => ((Math.Abs(tile.coordinates.x - currentTile.coordinates.x) >= 6 && 
                                                Math.Abs(tile.coordinates.x - currentTile.coordinates.x) <= 10) &&
                                                tile.coordinates.y == currentTile.coordinates.y) ||
                                                ((Math.Abs(tile.coordinates.y - currentTile.coordinates.y) >= 6 && 
                                                Math.Abs(tile.coordinates.y - currentTile.coordinates.y) <= 10) && 
                                                tile.coordinates.x == currentTile.coordinates.x);

        TilesInAttackRange = Array.FindAll(GridManager.Instance.Tiles, PredAttack);
        GridManager.Instance.ShowAttackArea(this);

        yield return new WaitUntil(() => targetPosition != new Vector2(-1, -1));

        //Attacking

        int attackDamage = LongshotDamage;

        Tile targetTile;
        GridManager.Instance.TilesByCoordinates.TryGetValue(targetPosition, out targetTile);

        if (targetTile.occupant != null && targetTile.occupant.GetType() == typeof(Enemy))
        {
            if (targetTile.occupant.Health - attackDamage <= 0)
            {
                enemyDown = true;
            }
            targetTile.occupant.Health -= attackDamage;
        }

        //Ends attack, checks if enemy is down to either continue chain attack or end turn

        GridManager.Instance.HideAttackArea(this);
        targetPosition = new Vector2(-1, -1);

        if (enemyDown == true)
        {
            state = UnitState.ReadyToAct;
            actionMenu.SetButtonControls(this, info);
            actionMenu.ShowMenu();
        }
        else
        {
            state = UnitState.Waiting;
            EndTurn();
        }

        yield return null;
    }

    public override void Action2()
    {
        actionMenu.HideMenu();
        StartCoroutine(Bullseye());
    }

    private IEnumerator Bullseye()
    {
        state = UnitState.MakingAction;

        //Selecting Target

        Predicate<Tile> PredAttack = (tile) => ((Math.Abs(tile.coordinates.x - currentTile.coordinates.x) >= 6 &&
                                                Math.Abs(tile.coordinates.x - currentTile.coordinates.x) <= 15) &&
                                                tile.coordinates.y == currentTile.coordinates.y) ||
                                                ((Math.Abs(tile.coordinates.y - currentTile.coordinates.y) >= 6 &&
                                                Math.Abs(tile.coordinates.y - currentTile.coordinates.y) <= 15) &&
                                                tile.coordinates.x == currentTile.coordinates.x);

        TilesInAttackRange = Array.FindAll(GridManager.Instance.Tiles, PredAttack);
        GridManager.Instance.ShowAttackArea(this);

        yield return new WaitUntil(() => targetPosition != new Vector2(-1, -1));

        //Attacking

        int attackDamage = BullseyeDamage;

        Tile targetTile;
        GridManager.Instance.TilesByCoordinates.TryGetValue(targetPosition, out targetTile);

        if (targetTile.occupant != null && targetTile.occupant.GetType() == typeof(Enemy))
        {
            targetTile.occupant.Health -= attackDamage;
        }

        //Ends turn

        GridManager.Instance.HideAttackArea(this);
        targetPosition = new Vector2(-1, -1);
        state = UnitState.Waiting;
        EndTurn();

        yield return null;
    }
}
