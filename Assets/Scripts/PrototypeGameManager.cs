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
    public static bool briefed = false;

    public int currentLevel = 0;

    public AudioSource voiceLines;
    public AudioClip missionStartSFX;
    public AudioClip missionBriefSFX;
    public AudioClip missionWinSFX;
    
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();

        FreezePlayer();

        if (!briefed)
        {
            AudioSource.PlayClipAtPoint(missionBriefSFX, Camera.main.transform.position);
            screenMessage.SetText("Flanagan: Alright Valentina, you know the mission. Get to the vault, we got your escape route covered. Good luck, friend.");
            Invoke("ClearMessage", 5);
        }
        else
        {
            AudioSource.PlayClipAtPoint(missionStartSFX, Camera.main.transform.position);
            screenMessage.SetText("Get to the vault and phase through its wall!");
            Invoke("ClearMessage", 3);
        }
    }

    public void SayLine(AudioClip clipSFX)
    {
        voiceLines.clip = clipSFX;
        voiceLines.Play();
    }

    public void ClearMessage()
    {
        if (!briefed)
        {
            briefed = true;
        }
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
        if (gameRunning)
        {
            gameRunning = false;
            SayLine(missionWinSFX);
            screenMessage.SetText("<color=green>" + yourMessage + "</color>");
            Invoke("ReloadGame", 4);
        }
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