using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    public string winMessage;
    public float winDelay = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PrototypeGameManager>().WinMessage("Flanagan: " + winMessage, winDelay);
        }
    }

    // Spectacular work, Valentina. We'll get you out of there shortly.
}
