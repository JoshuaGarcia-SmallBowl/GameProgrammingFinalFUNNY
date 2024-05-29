using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    public GameObject cactus;
    public GameObject mushroom;
    public GameObject boss;
    public int enemiesLeft = 1;

    public float spawnRange = 9;
    private int wave = 0;
    private int enemiesToSpawn = 2;
    private int score;

    private bool gameActive = false;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;
    public GameObject newWaveUI;

    private float waveInfoTime = 1.5f;
    private float waveInfoStart;
    private bool waveInfoUp;

    private bool StoryMode = false;
    private int bossCooldown = 10;
    public GameObject player;
    private PlayerController playerController;
    private AudioSource audioSource;

    public GameObject winScreen;

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
        if (gameActive)
        {
            
            if (!waveInfoUp)
            {
                enemiesLeft = FindObjectsOfType<Enemy>().Length;
                if (enemiesLeft == 0)
                {
                    //End Game if Story Mode, otherwise activate the new wave UI
                    if (StoryMode && wave == 10)
                    {
                        gameActive = false;
                        playerController.movability(false);
                        winScreen.SetActive(true);
                    }
                    else
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
            
        }
        if (waveInfoUp)
        {
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
        float spawnPosx = Random.Range(-spawnRange, spawnRange);
        float spawnPosz = Random.Range(-spawnRange - 80, spawnRange - 80);
        Vector3 returnPos = new Vector3(spawnPosx, 0, spawnPosz);
        return returnPos;
    }

    public void StartGame(bool story)
    {
        if (story)
        {
            StoryMode = true;
        }
        gameActive = true;
    }

    public void ChangeMusic()
    {
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
