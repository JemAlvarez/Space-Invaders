using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayBeforeLoad = 3f;

    private void Awake()
    {
        Screen.SetResolution(576, 1024, false);
        Screen.fullScreen = false;
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
    }

    public void LoadGameScene()
    {
        if (FindObjectOfType<GameSession>())
        {
            FindObjectOfType<GameSession>().ResetScore();
        }
        SceneManager.LoadScene("Game");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        SceneManager.LoadScene("Game Over");
    }
}
