using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PrototypeGameManager : MonoBehaviour
{
    public TextMeshProUGUI screenMessage;
    public bool playerAlive = true;
    public static bool gameRunning = false; // Game doesn't start running until message is done

    // Start is called before the first frame update
    void Start()
    {
        FreezePlayer();

        screenMessage.SetText("<color=white>OK Valentina, infiltrate the bank's vault. Go!</color>");
        Invoke("ClearMessage", 3);
    }

    public void ClearMessage()
    {
        screenMessage.SetText("");
        if (playerAlive && !gameRunning)
        {
            gameRunning = true;
            UnfreezePlayer();
        }
    }

    public void GameOverMessage(string yourMessage)
    {
        if (gameRunning)
        {
            FreezePlayer();
            gameRunning = false;
            screenMessage.SetText(yourMessage);
            Invoke("ReloadGame", 2);
        }
    }

    public void WinMessage(string yourMessage)
    {
        gameRunning = false;
        screenMessage.SetText("<color=green>" + yourMessage + "</color>");
        Invoke("ReloadGame", 2);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FreezePlayer()
    {
        FindObjectOfType<FPSController>().walkSpeed = 0;
        FindObjectOfType<FPSController>().runSpeed = 0;
    }

    public void UnfreezePlayer()
    {
        FindObjectOfType<FPSController>().walkSpeed = 12;
        FindObjectOfType<FPSController>().runSpeed = 24;
    }
}
