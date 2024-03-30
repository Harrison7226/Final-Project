using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    public GameObject player;
    public GameObject noteUI;
    public GameObject hud;
    public GameObject inv;

    public AudioSource paper;

    public string GetDescription()
    {
        return "Read Note";
    }

    public void Exit()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
        player.GetComponent<FPSController>().canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Interact()
    {
        noteUI.SetActive(true);
        hud.SetActive(false);
        inv.SetActive(false);
        player.GetComponent<FPSController>().canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        paper.Play();
    }
}
