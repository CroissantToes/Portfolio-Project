using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Berserker : Hero
{
    [Header("Attack Info")]
    [SerializeField] int IronWindDamage = 2;
    [SerializeField] int CleaveDamage = 4;

    public override void Action1()
    {
        actionMenu.HideMenu();
        StartCoroutine(IronWind());
    }

    private IEnumerator IronWind()
    {
        autoAttacking = true;
        bool enemyDown = false;
        state = UnitState.MakingAction;

        //Selecting Target

        Predicate<Tile> PredAttack = (tile) => (Math.Abs(tile.coordinates.x - currentTile.coordinates.x) <= 1 &&
                                                Math.Abs(tile.coordinates.y - currentTile.coordinates.y) <= 1 &&
                                                tile != currentTile);

        TilesInAttackRange = Array.FindAll(GridManager.Instance.Tiles, PredAttack);
        GridManager.Instance.ShowAttackArea(this);

        yield return new WaitForSeconds(0.5f);

        //Attacking

        int attackDamage = IronWindDamage;

        foreach(Tile tile in TilesInAttackRange )
        {
            if (tile.occupant != null && tile.occupant.GetType() == typeof(Enemy))
            {
                if (tile.occupant.Health - attackDamage <= 0)
                {
                    enemyDown = true;
                }
                tile.occupant.Health -= attackDamage;
            }
        }

        //Ends attack, checks if enemy is down to either continue chain attack or end turn

        autoAttacking = false;
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
        StartCoroutine(Cleave());
    }

    private IEnumerator Cleave()
    {
        state = UnitState.MakingAction;

        //Selecting Target

        Predicate<Tile> PredAttack = (tile) => (Math.Abs(tile.coordinates.x - currentTile.coordinates.x) == 1 && tile.coordinates.y == currentTile.coordinates.y) ||
                                         (Math.Abs(tile.coordinates.y - currentTile.coordinates.y) == 1 && tile.coordinates.x == currentTile.coordinates.x);

        TilesInAttackRange = Array.FindAll(GridManager.Instance.Tiles, PredAttack);
        GridManager.Instance.ShowAttackArea(this);

        yield return new WaitUntil(() => targetPosition != new Vector2(-1, -1));

        //Attacking

        int attackDamage = CleaveDamage;

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