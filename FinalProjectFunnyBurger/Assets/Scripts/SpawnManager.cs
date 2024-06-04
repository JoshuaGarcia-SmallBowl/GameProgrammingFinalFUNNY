using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    //enemies
    public GameObject cactus;
    public GameObject mushroom;
    public GameObject boss;
    public int enemiesLeft = 1;

    //spawning variables
    public float spawnRange = 9;
    private int wave = 0;
    private int enemiesToSpawn = 2;
    private int score;

    private bool gameActive = false;

    //UI
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;
    public GameObject newWaveUI;

    //wave info display
    private float waveInfoTime = 1.5f;
    private float waveInfoStart;
    private bool waveInfoUp;

    private int bossCooldown = 10;

    //external references
    public GameObject player;
    private PlayerController playerController;
    private AudioSource audioSource;

    //music
    public AudioClip waveOneMusic;
    public AudioClip waveTenMusic;
    public AudioClip waveTwentyMusic;
    public AudioClip waveThirthyMusic;
    
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //new wave spawning
        if (gameActive)
        {
            if (!waveInfoUp)
            {
                //check how many enemies are alive
                enemiesLeft = FindObjectsOfType<Enemy>().Length;
                if (enemiesLeft == 0)
                {
                    
                        wave++;
                        bossCooldown--;
                        newWaveUI.SetActive(true);
                        score += wave * 100;
                        waveText.text = ("Wave: " + wave);
                        scoreText.text = ("Score: " + score);
                        waveInfoStart = Time.time;
                        waveInfoUp = true;
                        ChangeMusic();
                    
                    

                }
            }
            
        }
        if (waveInfoUp)
        {
            //remove the wave info after enough time
            float InfoTime = Time.time - waveInfoStart;
            if (InfoTime > waveInfoTime)
            {
                EndInfo();

            }
        }
        
    }

    void EndInfo()
    {
        newWaveUI.SetActive(false);
        waveInfoUp = false;
        enemiesToSpawn = wave + 5;
        SpawnWave(enemiesToSpawn);
    }

    void SpawnWave(int enemies)
    {
        for (int i = 0; i < enemies; i++) 
        {
            if (Random.Range(1, 10) <= 5)
            {
                Instantiate(cactus, GenerateSpawnPosition(), Quaternion.identity);
            }
            else
            {
                Instantiate(mushroom, GenerateSpawnPosition(), Quaternion.identity);
            }
            
        }
        if (bossCooldown == 0)
        {
            bossCooldown = 10;
            Instantiate(boss, GenerateSpawnPosition(), Quaternion.identity);
            score += wave * 500;
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        //generate a random spawn position for the enemies
        float spawnPosx = Random.Range(-spawnRange, spawnRange);
        float spawnPosz = Random.Range(-spawnRange - 80, spawnRange - 80);
        Vector3 returnPos = new Vector3(spawnPosx, 0, spawnPosz);
        return returnPos;
    }

    //called by gamemanager
    public void StartGame()
    {
        gameActive = true;
    }

    public void ChangeMusic()
    {
        //change the music on certain waves
        if (wave == 30)
        {
            audioSource.clip = waveThirthyMusic;
            audioSource.Play();

        }
        else if (wave == 20)
        {
            audioSource.clip = waveTwentyMusic;
            audioSource.Play();
        }
        else if (wave == 10)
        {
            audioSource.clip = waveTenMusic;
            audioSource.Play();
        }
        else if (wave == 1)
        {
            
            audioSource.clip = waveOneMusic;
            audioSource.Play();
            
        }
    }

    
}
