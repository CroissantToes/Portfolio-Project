using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager Instance { get; private set; }
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

    public GameObject pauseMenu = null;
    public GameObject winMenu = null;
    public GameObject loseMenu = null;
    private bool win = false;
    private bool lose = false;

    private void Update()
    {
        if((Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P)) && win == false && lose == false)
        {
            ShowPauseMenu();
        }
    }

    public void ShowWinMenu()
    {
        FreezeGame();
        winMenu.SetActive(true);
        win = true;
    }
    
    public void ShowLoseMenu()
    {
        FreezeGame();
        loseMenu.SetActive(true);
        lose = true;
    }

    public void ShowPauseMenu()
    {
        FreezeGame();
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        UnfreezeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void FreezeGame()
    {
        Time.timeScale = 0f;
    }

    private void UnfreezeGame()
    {
        Time.timeScale = 1f;
    }
}
