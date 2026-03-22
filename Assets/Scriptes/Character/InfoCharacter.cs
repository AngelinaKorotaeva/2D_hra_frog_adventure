using UnityEngine;

public class InfoCharacter : MonoBehaviour
{
    public static InfoCharacter Instance;

    private int levelCompleted;
    private int coinsCompleted;
    private int coins;
    private int scoreCompleted;
    private int score;

    private int attackValue = 1;
    private int lives = 3;

    private float totalLives = 0.5f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject); 
        }

    }

    public int LevelCompleted
    {
        get => levelCompleted;
        set
        {
            levelCompleted = value;
            SaveData();
        }
    }
    public int CoinsCompleted
    {
        get => coinsCompleted;
        set
        {
            coinsCompleted = value;
            SaveData();
        }
    }
    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            SaveData();
        }
    }
    public int ScoreCompleted
    {
        get => score;
        set
        {
            score = value;
            SaveData();
        }
    }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            SaveData();
        }
    }
    public int AttackValue
    {
        get => attackValue;
        set
        {
            attackValue = value;
            SaveData();
        }
    }
    public int Lives
    {
        get => lives;
        set { lives = value; SaveData(); }
    }

    public float TotalLives
    {
        get => totalLives;
        set { totalLives = value; SaveData(); }
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("LevelCompleted", levelCompleted);
        PlayerPrefs.SetInt("CoinsCompleted", coinsCompleted);
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("ScoreCompleted", scoreCompleted);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("AttackValue", attackValue);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.SetFloat("TotalLives", totalLives);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        levelCompleted = PlayerPrefs.GetInt("LevelCompleted", 0);
        coinsCompleted = PlayerPrefs.GetInt("CoinsCompleted", 0);
        coins = PlayerPrefs.GetInt("Coins", 0);
        scoreCompleted = PlayerPrefs.GetInt("ScoreCompleted", 0);
        score = PlayerPrefs.GetInt("Score", 0);
        attackValue = PlayerPrefs.GetInt("AttackValue", 1);
        lives = PlayerPrefs.GetInt("Lives", 3);
        totalLives = PlayerPrefs.GetFloat("TotalLives", 0.5f);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save();

        levelCompleted = 0;
        coinsCompleted = 0;
        coins = 0;
        scoreCompleted = 0;
        score = 0;
        attackValue = 1;
        lives = 3;
        totalLives = 0.5f;
        SaveData(); 
    }
    private void OnApplicationQuit()
    {
        ResetData();
    }
}
