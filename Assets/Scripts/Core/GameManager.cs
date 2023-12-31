using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Speed")]
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; private set; }

    [Header("UI")]
    public GameObject gameOverScreen;
    public Text coinsText;
    public Text scoreText;
    public Text hiscoreText;

    private Spawner spawnerGround;
    private SpawnerWall spawnerWall;
    private SpawnerCoins spawnerCoins;

    private float score;
    private float coin;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        
        if (Instance != null)
            DestroyImmediate(gameObject);
        else
            Instance = this;
    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }

    private void Start()
    {
        spawnerGround = FindObjectOfType<Spawner>();
        spawnerWall = FindObjectOfType<SpawnerWall>();
        spawnerCoins = FindObjectOfType<SpawnerCoins>();

        score = 0f;
        enabled = true;
        GameSpeed = initialGameSpeed;
        UpdateHighScore();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        GameSpeed = 0f;
        enabled = false;
        gameOverScreen.SetActive(true);
        spawnerGround.gameObject.SetActive(false);
        spawnerWall.gameObject.SetActive(false);
        spawnerCoins.gameObject.SetActive(false);
        UpdateHighScore();
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        GameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += GameSpeed * Time.deltaTime;
        coinsText.text = coin.ToString();
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    public void IncreaseCoin()
    {
        coin++;
    }

    private void UpdateHighScore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
