using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    void Start()
    {
        Invoke("MainMenu", 5);
    }

    void MainMenu()
    {
        SceneManager.LoadScene(0);
        PrototypeGameManager.currentLevel = 1;
        PrototypeGameManager.gameRunning = false;
        PrototypeGameManager.briefed = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 7.0f, 0));
    }
}
