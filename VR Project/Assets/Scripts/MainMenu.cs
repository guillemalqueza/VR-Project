using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void Level1()
    {
        SceneManager.LoadScene("GameScene");
    }

}
