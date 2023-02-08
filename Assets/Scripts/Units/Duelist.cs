using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Duelist : Hero
{
    [Header("Attack Info")]
    [SerializeField] int SideStepBaseDamage = 1;
    [SerializeField] int SideStepPerfectDamage = 2;
    [SerializeField] int LungeDamage = 3;

    public override void Action1()
    {
        actionMenu.HideMenu();
        StartCoroutine(SideStep());
    }

    private IEnumerator SideStep()
    {
        bool enemyDown = false;

        //Moving

        makingMoveAsAction = true;
        ChooseMove();
        Predicate<Tile> PredMove = (tile) => (Math.Abs(tile.coordinates.x - currentTile.coordinates.x) == 1 && Math.Abs(tile.coordinates.y - currentTile.coordinates.y) == 1) && tile.isObstructed == false;

        TilesInMoveRange = Array.FindAll(GridManager.Instance.Tiles, PredMove);
        GridManager.Instance.ShowMoveArea(this);

        yield return new WaitUntil(() => state == UnitState.MakingAction);

        //Selecting Target

        makingMoveAsAction = false;

        Predicate<Tile> PredAttack = (tile) => (Math.Abs(tile.coordinates.x - currentTile.coordinates.x) == 1 && tile.coordinates.y == currentTile.coordinates.y) ||
                                         (Math.Abs(tile.coordinates.y - currentTile.coordinates.y) == 1 && tile.coordinates.x == currentTile.coordinates.x);

        TilesInAttackRange = Array.FindAll(GridManager.Instance.Tiles, PredAttack);
        GridManager.Instance.ShowAttackArea(this);

        yield return new WaitUntil(() => targetPosition != new Vector2(-1, -1));

        //Attacking

        int attackDamage = SideStepPerfectDamage;
        if (targetPosition.y > currentTile.coordinates.y)
        {
            attackDamage = SideStepBaseDamage;
        }

        Tile targetTile;
        GridManager.Instance.TilesByCoordinates.TryGetValue(targetPosition, out targetTile);

        if (targetTile.occupant != null && targetTile.occupant.GetType() == typeof(Enemy))
        {
            if(targetTile.occupant.Health - attackDamage <= 0)
            {
                enemyDown = true;
            }
            targetTile.occupant.Health -= attackDamage;
        }

        //Ends attack, checks if enemy is down to either continue chain attack or end turn

        GridManager.Instance.HideAttackArea(this);
        targetPosition = new Vector2(-1, -1);

        if(enemyDown == true)
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
        StartCoroutine(Lunge());
    }

    private IEnumerator Lunge()
    {
        state = UnitState.MakingAction;

        //Selecting Target

        Predicate<Tile> PredAttack = (tile) => (Math.Abs(tile.coordinates.x - currentTile.coordinates.x) == 2 && tile.coordinates.y == currentTile.coordinates.y) ||
                                         (Math.Abs(tile.coordinates.y - currentTile.coordinates.y) == 2 && tile.coordinates.x == currentTile.coordinates.x);

        Tile[] potentialTiles = Array.FindAll(GridManager.Instance.Tiles, PredAttack);
        List<Tile> tilesToUse = new List<Tile>();

        foreach(Tile tile in potentialTiles)
        {
            tilesToUse.Add(tile);
            Predicate<Tile> PredBetween = (t) => (t.coordinates.x > currentTile.coordinates.x && t.coordinates.x < tile.coordinates.x && t.coordinates.y == currentTile.coordinates.y) ||
                                                 (t.coordinates.x < currentTile.coordinates.x && t.coordinates.x > tile.coordinates.x && t.coordinates.y == currentTile.coordinates.y) ||
                                                 (t.coordinates.y > currentTile.coordinates.y && t.coordinates.y < tile.coordinates.y && t.coordinates.x == currentTile.coordinates.x) ||
                                                 (t.coordinates.y < currentTile.coordinates.y && t.coordinates.y > tile.coordinates.y && t.coordinates.x == currentTile.coordinates.x);
            Tile[] tilesToCheck = Array.FindAll(GridManager.Instance.Tiles, PredBetween);
            foreach(Tile check in tilesToCheck)
            {
                if(check.isObstructed == true)
                {
                    tilesToUse.Remove(tile);
                }
            }
        }

        TilesInAttackRange = tilesToUse.ToArray();

        GridManager.Instance.ShowAttackArea(this);

        yield return new WaitUntil(() => targetPosition != new Vector2(-1, -1));

        //Moving

        Tile targetTile;
        GridManager.Instance.TilesByCoordinates.TryGetValue(targetPosition, out targetTile);
        
        Tile moveTile = null;
        Vector2 coords = new Vector2(0,0);

        if(targetTile.coordinates.x < currentTile.coordinates.x)
        {
            coords = new Vector2(currentTile.coordinates.x - 1, currentTile.coordinates.y);
            GridManager.Instance.TilesByCoordinates.TryGetValue(coords, out moveTile); 
        }
        else if(targetTile.coordinates.x > currentTile.coordinates.x)
        {
            coords = new Vector2(currentTile.coordinates.x + 1, currentTile.coordinates.y);
            GridManager.Instance.TilesByCoordinates.TryGetValue(coords, out moveTile);
        }
        else if (targetTile.coordinates.y < currentTile.coordinates.y)
        {
            coords = new Vector2(currentTile.coordinates.x, currentTile.coordinates.y - 1);
            GridManager.Instance.TilesByCoordinates.TryGetValue(coords, out moveTile);
        }
        else if (targetTile.coordinates.y > currentTile.coordinates.y)
        {
            coords = new Vector2(currentTile.coordinates.x, currentTile.coordinates.y + 1);
            GridManager.Instance.TilesByCoordinates.TryGetValue(coords, out moveTile);
        }

        MoveToTile(moveTile);

        //Attacking

        if (targetTile.occupant != null && targetTile.occupant.GetType() == typeof(Enemy))
        {
            targetTile.occupant.Health -= LungeDamage;
        }

        //Ending turn

        GridManager.Instance.HideAttackArea(this);

        targetPosition = new Vector2(-1, -1);
        state = UnitState.Waiting;
        EndTurn();

        yield return null;
    }
}