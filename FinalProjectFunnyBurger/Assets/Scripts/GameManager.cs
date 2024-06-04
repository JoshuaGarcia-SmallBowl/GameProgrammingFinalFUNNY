using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //UI
    public GameObject GOUI;
    public GameObject TitleUI;
    public GameObject pauseUI;

    //external references
    private SpawnManager spawnManager;
    private PlayerController playerController;
    private GameObject player;

    private bool gameActive = false;
    private bool paused = false;
    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        //pausing
        if (gameActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused)
                {
                    paused = false;
                    Time.timeScale = 1.0f;
                    pauseUI.SetActive(false);
                }
                else
                {
                    paused = true;
                    Time.timeScale = 0.0f;
                    pauseUI.SetActive(true);
                }
            }
        }
        
    }

    public void StartEndless()
    {
        //start the game
        TitleUI.SetActive(false);
        spawnManager.StartGame();
        playerController.movability(true);
        gameActive = true;
    }

    //used by other scripts
    public void GameOver()
    {
        GOUI.SetActive(true);
        gameActive = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }
}
