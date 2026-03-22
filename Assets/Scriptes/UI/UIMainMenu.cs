using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    public void StartGame()
    {
        int completed = InfoCharacter.Instance.LevelCompleted;
        if (completed == 0)
        {
            SceneManager.LoadScene(2);
            Time.timeScale = 1;
        }
        else if (completed == 1)
        {
            SceneManager.LoadScene(3);
            Time.timeScale = 1;
        }
        else if (completed == 2)
        {
            SceneManager.LoadScene(4);
            Time.timeScale = 1;
        }
        else
        {
            SceneManager.LoadScene(2);
            Time.timeScale = 1;
        }
        Time.timeScale = 1;
    }
    
    public void Levels()
    {
        SceneManager.LoadScene(1);
    }
    public void Shop()
    {
        SceneManager.LoadScene(5);
    }
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
