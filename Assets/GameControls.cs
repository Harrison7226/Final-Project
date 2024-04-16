using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    public GameObject controlsPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            controlsPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            controlsPanel.SetActive(false);
        }
    }
}
