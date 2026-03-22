using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class UIManager:MonoBehaviour
{
    [Header("Game over & Finish")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private TextMeshProUGUI scoreValue;

    [SerializeField] private GameObject star_1;
    [SerializeField] private GameObject star_2;
    [SerializeField] private GameObject star_3;


    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;

    private Animator animator;
    private void Awake()
    {
        animator = gameOverScreen.GetComponent<Animator>();
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    #region GAME_OVER FINISH MENU
     public void GameOver()
    {
        gameOverScreen.SetActive(true);
        InfoCharacter.Instance.Score = 0;
        animator.SetTrigger("GameOver");
        //StartCoroutine(PauseAfterAnim()); 
    }
    private IEnumerator PauseAfterAnim()
    {
        yield return null; 
        Time.timeScale = 0;
    }
    public void FinichScreen()
    {
        finishScreen.SetActive(true);
        scoreValue.SetText("" + InfoCharacter.Instance.Score);
        int score = InfoCharacter.Instance.Score;
        if (score >= 1370)
        {
            star_3.SetActive(true);
            star_2.SetActive(false);
            star_1.SetActive(false);
            InfoCharacter.Instance.Coins += 15;
        }
        else if (score >= 1000)
        {
            star_3.SetActive(false);
            star_2.SetActive(true);
            star_1.SetActive(false);
            InfoCharacter.Instance.Coins += 6;
        }
        else
        {
            star_3.SetActive(false);
            star_2.SetActive(false);
            star_1.SetActive(true);
        }
        InfoCharacter.Instance.Score = 0;
        Time.timeScale = 0;
    }
    public void Restart()
    {
        if (InfoCharacter.Instance != null)
        {
            InfoCharacter.Instance.Coins = 0;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }
    public void Levels()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        if (InfoCharacter.Instance != null)
        {
            InfoCharacter.Instance.Coins = 0;
        }
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
#endregion

    #region PAUSE MENU

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        Time.timeScale = status ? 0 : 1;
    }

    #endregion
}
