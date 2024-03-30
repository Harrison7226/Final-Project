using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour, IInteractable
{
    public GameObject player;
    public GameObject keypadUI;
    public GameObject hud;
    public GameObject inv;

    public GameObject vaultDoor;
    public Animator animator;

    public TextMeshProUGUI typedText;
    public string answer;

    public AudioSource button;
    public AudioSource correct;
    public AudioSource wrong;

    void Start()
    {
        keypadUI.SetActive(false);
        typedText.text = "";
    }


    public void Number(int number)
    {
        // Limit the number of numbers that can be entered
        if (typedText.text.Length < answer.Length)
        {
            typedText.text += number.ToString();
            button.Play();
        }
    }

    public void Enter()
    {
        if (typedText.text == answer)
        {
            correct.Play();
            vaultDoor.GetComponent<Animator>().SetBool("open", true);
        }
        else
        {
            wrong.Play();
        }
        typedText.text = "";
    }

    public void Delete()
    {
        if (typedText.text.Length > 0)
        {
            typedText.text = typedText.text.Substring(0, typedText.text.Length - 1);
            button.Play();
        }
    }

    public void Exit()
    {
        keypadUI.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
        player.GetComponent<FPSController>().canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Interact()
    {
        keypadUI.SetActive(true);
        hud.SetActive(false);
        inv.SetActive(false);
        player.GetComponent<FPSController>().canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public string GetDescription()
    {
        return "Enter Code";
    }
}
