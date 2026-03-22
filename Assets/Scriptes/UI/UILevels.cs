using UnityEngine;
using UnityEngine.SceneManagement;

public class UILevels : MonoBehaviour
{
    [SerializeField] private GameObject levelsScreen;

    public void Level1()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    public void Level2()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }
    public void Level3()
    {
        SceneManager.LoadScene(4);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
