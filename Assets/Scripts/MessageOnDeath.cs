using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageOnDeath : MonoBehaviour
{
    public string message;
    public TextMeshProUGUI screenMessage;
    private bool messageSaid = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!messageSaid && !GetComponent<PrototypeEnemyBehaviour>().alive)
        {
            screenMessage.SetText(message);
            messageSaid = true;
            Invoke("ClearMessage", 5);
        }

    }

    public void ClearMessage()
    {
        if (PrototypeGameManager.gameRunning)
        {
            screenMessage.SetText("");
        }

    }
}
