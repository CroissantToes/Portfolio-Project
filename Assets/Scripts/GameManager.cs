using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<Hero> heroesInPlay;
    private List<Enemy> enemiesInPlay;
    private Barrier princeBarrier;
    [HideInInspector] public Unit selectedUnit;
    public GameState state { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text roundTimer;

    private void Start()
    {
        heroesInPlay = new List<Hero>();
        enemiesInPlay = new List<Enemy>();
        princeBarrier = FindObjectOfType<Barrier>();
        princeBarrier.BarrierDestroyed += EndGame;
        state = GameState.playerturn;
        StartCoroutine(PlayerTurn());
    }

    private void EndGame(object sender, EventArgs e)
    {
        CheckGameState();
    }

    private IEnumerator GameStart()
    {
        yield return null;
    }

    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player turn start");
        state = GameState.playerturn;
        Hero[] ActiveHeroes = FindObjectsOfType<Hero>();

        foreach (Hero hero in ActiveHeroes)
        {
            heroesInPlay.Add(hero);
            hero.canPlay = true;
            hero.hasMoved = false;
        }

        yield return new WaitUntil(() => heroesInPlay.Count == 0);

        Debug.Log("Player turn end");
        StartCoroutine(EnemyTurn());

        yield return null;
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy turn start");
        state = GameState.enemyturn;

        Enemy[] ActiveEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in ActiveEnemies)
        {
            enemiesInPlay.Add(enemy);
            enemy.canPlay = true;
        }

        foreach(Enemy enemy in enemiesInPlay)
        {
            enemy.StartTurn();
            yield return new WaitWhile(() => enemy.canPlay == true);
        }

        enemiesInPlay.Clear();

        Debug.Log("Enemy turn end");
        StartCoroutine(PlayerTurn());

        yield return null;
    }

    private IEnumerator RoundStart()
    {
        yield return null;
    }

    private IEnumerator Lose()
    {
        Debug.Log("Player lost");
        yield return null;
    }

    private IEnumerator Win()
    {
        Debug.Log("Player won");
        yield return null;
    }

    private void SpawnNextWave()
    {

    }

    private void CheckGameState()
    {
        if(princeBarrier.Health <= 0 || heroesInPlay.Count == 0)
        {
            StopAllCoroutines();
            state = GameState.lose;
            StartCoroutine(Lose());
            princeBarrier.BarrierDestroyed -= EndGame;
        }
        else if(numberOfWavesRemaining == 0 && enemiesInPlay.Count == 0)
        {
            StopAllCoroutines();
            state = GameState.win;
            StartCoroutine(Win());
        }
    }

    public void RemoveHeroFromPlay(Hero hero)
    {
        heroesInPlay.Remove(hero);
    }

    public void RemoveEnemyFromPlay(Enemy enemy)
    {
        enemiesInPlay.Remove(enemy);
    }
}
