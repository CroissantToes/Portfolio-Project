using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [Header("Game Parameters")]
    [SerializeField] int roundsUntilNextWave;
    [SerializeField] int numberOfWavesRemaining;
    private List<Hero> allHeroes = new List<Hero>();
    private List<Hero> heroesInPlay = new List<Hero>();
    private List<Enemy> allEnemies = new List<Enemy>();
    private List<Enemy> enemiesInPlay = new List<Enemy>();
    private Barrier princeBarrier;
    [HideInInspector] public Unit selectedUnit;
    public GameState state { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text roundTimer;

    private void Start()
    {
        allHeroes = FindObjectsOfType<Hero>().ToList();
        allEnemies = FindObjectsOfType<Enemy>().ToList();
        princeBarrier = FindObjectOfType<Barrier>();
        princeBarrier.BarrierDestroyed += EndGame;
        state = GameState.playerturn;;
    }

    public void StartGame()
    {
        HUDManager.Instance.SetEnemyCounter(allEnemies.Count);
        StartCoroutine(PlayerTurn());
    }

    private void EndGame(object sender, EventArgs e)
    {
        CheckGameState();
    }

    private IEnumerator GameStart()
    {
        state = GameState.gamestart;
        yield return null;
    }

    private IEnumerator PlayerTurn()
    {
        state = GameState.playerturn;

        HUDManager.Instance.ShowPrompter("It's Your Turn!");

        yield return new WaitForSeconds(1f);

        HUDManager.Instance.ClosePrompter();

        foreach(Hero hero in allHeroes)
        {
            heroesInPlay.Add(hero);
            hero.SetMoveArea();
            hero.state = UnitState.ReadyToMove;
        }

        foreach(Enemy enemy in allEnemies)
        {
            enemy.SetMoveArea();
        }

        HUDManager.Instance.ShowSkipButton();

        yield return new WaitUntil(() => heroesInPlay.Count == 0);

        StartCoroutine(EnemyTurn());

        yield return null;
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);

        bool isOver = CheckGameState();

        yield return new WaitUntil(() => isOver == false);

        HUDManager.Instance.ShowPrompter("Enemy Turn...");

        yield return new WaitForSeconds(1f);

        HUDManager.Instance.ClosePrompter();
        HUDManager.Instance.HideSkipButton();
        state = GameState.enemyturn;

        foreach (Enemy enemy in allEnemies)
        {
            enemiesInPlay.Add(enemy);
            enemy.state = UnitState.ReadyToMove;
        }

        foreach(Enemy enemy in enemiesInPlay)
        {
            enemy.StartTurn();
            yield return new WaitUntil(() => enemy.state == UnitState.Waiting);
        }

        enemiesInPlay.Clear();

        StartCoroutine(RoundStart());

        yield return null;
    }

    private IEnumerator RoundStart()
    {
        state = GameState.roundstart;
        selectedUnit = null;
        bool isOver = CheckGameState();

        if(isOver == false)
        {
            StartCoroutine(PlayerTurn());
        }
        yield return null;
    }

    private IEnumerator Lose()
    {
        MenuManager.Instance.ShowLoseMenu();
        yield return null;
    }

    private IEnumerator Win()
    {
        MenuManager.Instance.ShowWinMenu();
        yield return null;
    }

    private void SpawnNextWave()
    {

    }

    private bool CheckGameState()
    {
        HUDManager.Instance.SetEnemyCounter(allEnemies.Count);
        bool isOver = false;

        //If all heroes are down, lose
        if(allHeroes.Count == 0)
        {
            isOver = true;
            StopAllCoroutines();
            state = GameState.lose;
            StartCoroutine(Lose());
            princeBarrier.BarrierDestroyed -= EndGame;
        }

        //If all heroes down or barrier destroyed, lose
        if(princeBarrier.Health <= 0)
        {
            isOver = true;
            StopAllCoroutines();
            state = GameState.lose;
            StartCoroutine(Lose());
            princeBarrier.BarrierDestroyed -= EndGame;
        }

        //If all enemies are defeated, win
        else if(allEnemies.Count == 0)
        {
            isOver = true;
            StopAllCoroutines();
            state = GameState.win;
            StartCoroutine(Win());
        }

        return isOver;
    }

    public void RemoveHeroFromPlay(Hero hero)
    {
        heroesInPlay.Remove(hero);
    }

    public void RemoveEnemyFromPlay(Enemy enemy)
    {
        enemiesInPlay.Remove(enemy);
    }

    public void RemoveHeroFromPool(Hero hero)
    {
        allHeroes.Remove(hero);
        CheckGameState();
    }

    public void RemoveEnemyFromPool(Enemy enemy)
    {
        allEnemies.Remove(enemy);
        CheckGameState();
    }

    //Ends player turn early
    public void SkipPlayerTurn()
    {
        StopAllCoroutines();

        foreach(Hero hero in heroesInPlay)
        {
            hero.actionMenu.HideMenu();
            GridManager.Instance.HideMoveArea(hero);
            hero.state = UnitState.Waiting;
        }
        heroesInPlay.Clear();

        Debug.Log("Player turn end");
        StartCoroutine(EnemyTurn());
    }
}
