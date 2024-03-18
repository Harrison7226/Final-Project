using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PrototypeGameManager>().WinMessage("Flanagan: Spectacular work, Valentina. We'll get you out of there shortly.");
        }
    }
}
