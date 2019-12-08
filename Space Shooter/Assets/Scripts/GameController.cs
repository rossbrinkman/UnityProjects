using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public BGScroller bgscroller;
    public GameObject particleSystem;
    public ParticleSystem starfield;
    public ParticleSystem starfieldDistant;
    public AudioClip loseMusic;
    public AudioClip winMusic;
    public Mover asteroid1;
    public Mover asteroid2;
    public Mover asteroid3;
    public Mover enemyProjectile;
    public Mover enemy;

    public Text infoText;
    public Text scoreToWinText;
    public Text ScoreText;
    public Text restartText;
    public Text gameOverText;
    public Text timeAttackText;
    public Text hardModeText;
    public Text timeLeft;

    private float winValue = 100;
    private int score;
    public bool won;
    private bool hardMode = false;
    private bool timeAttack = false;
    private bool gameOver;
    private bool restart;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        restart = false;
        won = false;
        restartText.text = "";
        gameOverText.text = "";
        hardModeText.text = "Press 'H' for Hard Mode";
        timeAttackText.text = "Press 'T' for Time Attack Mode";
        scoreToWinText.text = "Score to Win: " + winValue;
        infoText.text = "";
        timeLeft.text = "";
        timeLeft.text = "";
        waveWait = 4;
        asteroid1.GetComponent<DestroyByContact>().enabled = true;
        asteroid2.GetComponent<DestroyByContact>().enabled = true;
        asteroid3.GetComponent<DestroyByContact>().enabled = true;
        enemyProjectile.GetComponent<DestroyByContact>().enabled = true;
        enemy.GetComponent<DestroyByContact>().enabled = true;
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
        audioSource = GetComponent<AudioSource>();
    }

     void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("Main");
            }
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (!hardMode && !timeAttack && !gameOver && Input.GetKeyDown(KeyCode.H))
        {            
            HardMode();
            hardMode = true;
        }
        if (hardMode && Input.GetKeyDown(KeyCode.N))
        {
            ExitHardMode();
        }
        if (!timeAttack && !hardMode && !gameOver && Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(TimeAttack());
            timeAttack = true;
        }

    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true) { 
            for (int i = 0; i < hazardCount; i++) {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
                yield return new WaitForSeconds(waveWait);
            if (gameOver)
            {                
                restartText.text = "Press 'K' to Restart";
                restart = true;
                break;
            }
        }
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void HardMode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);
        waveWait = waveWait * .5f;
        asteroid1.speed *= 2;
        asteroid2.speed *= 2;
        asteroid3.speed *= 2;

        enemy.speed *= 2;
        hazardCount *= 2;
        hardModeText.text = "Press 'N' for Normal Mode";
    }
    void ExitHardMode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);
        waveWait = waveWait / .5f;
        asteroid1.speed /= 2;
        asteroid2.speed /= 2;
        asteroid3.speed /= 2;

        enemy.speed /= 2;
        hazardCount /= 2;
        hardMode = false;
        hardModeText.text = "Press 'H' for Hard Mode";
    }

    IEnumerator TimeAttack()
    {
        DestroyEnemiesInScene();
        
            hazardCount = hazardCount * 5;
            waveWait = 1;
            asteroid1.GetComponent<DestroyByContact>().enabled = false;
            asteroid2.GetComponent<DestroyByContact>().enabled = false;
            asteroid3.GetComponent<DestroyByContact>().enabled = false;
            enemyProjectile.GetComponent<DestroyByContact>().enabled = false;
            enemy.GetComponent<DestroyByContact>().enabled = false;
            timeAttackText.text = "Press 'N' for Normal Mode";
        StartCoroutine(InfoText());
            winValue = 750;
            scoreToWinText.text = "Score to Win: " + winValue;
        float countdownValue = 30f;
        while (countdownValue >= 0)
        {
            timeLeft.text = "Back to normal mode in: " + countdownValue;
            yield return new WaitForSeconds(1.0f);
            countdownValue--;
        }
        DestroyEnemiesInScene();
        timeLeft.text = "";
        hazardCount = hazardCount / 5;
        waveWait = 4;
        asteroid1.GetComponent<DestroyByContact>().enabled = true;
        asteroid2.GetComponent<DestroyByContact>().enabled = true;
        asteroid3.GetComponent<DestroyByContact>().enabled = true;
        enemyProjectile.GetComponent<DestroyByContact>().enabled = true;
        enemy.GetComponent<DestroyByContact>().enabled = true;
        timeAttackText.text = "Press 'T' for Time Attack Mode";
        
    }

    IEnumerator InfoText()
    {
        infoText.text = "Time Attack:\nScore as many points as you can!";
        yield return new WaitForSeconds(5);
        infoText.text = "";
    }

    void DestroyEnemiesInScene()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
        if (!won && score >= winValue)
        {
            won = true;
            gameOverText.text = "You win!\n\nGAME CREATED BY\nROSS BRINKMAN";
            audioSource.clip = winMusic;
            audioSource.Play();
            if (hardMode)
            {
                ExitHardMode();
            }

            bgscroller.scrollSpeed = bgscroller.scrollSpeed * 36f;
            starfield.startSpeed = starfield.startSpeed * 60f;
            starfieldDistant.startSpeed = starfieldDistant.startSpeed * 60f;
            starfield.emissionRate = 30f;
            starfieldDistant.emissionRate = 45f;
            particleSystem.SetActive(false);
            particleSystem.SetActive(true);
            gameOver = true;
            restart = true;
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        scoreToWinText.text = "";
        hardModeText.text = "";
        timeAttackText.text = "";
        audioSource.clip = loseMusic;
        audioSource.Play();
        if (hardMode)
        {
            ExitHardMode();
        }
        gameOver = true;
    }
}
