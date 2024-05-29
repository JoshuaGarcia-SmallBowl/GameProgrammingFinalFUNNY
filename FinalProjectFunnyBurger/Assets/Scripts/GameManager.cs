using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GOUI;
    public GameObject TitleUI;

    private SpawnManager spawnManager;
    private PlayerController playerController;
    private GameObject player;
     
    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    public void StartEndless()
    {
        TitleUI.SetActive(false);
        spawnManager.StartGame(false);
        playerController.movability(true);
    }

    public void StartStory()
    {
        TitleUI.SetActive(false);
        spawnManager.StartGame(true);
        playerController.movability(true);
    }
    
    public void GameOver()
    {
        GOUI.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }
}
