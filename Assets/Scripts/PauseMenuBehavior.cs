using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehavior : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenu;
    public GameObject gameCanvas;

    void Awake()
    {
        isGamePaused = false;
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameCanvas.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameCanvas.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
