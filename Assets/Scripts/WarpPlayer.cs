using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpPlayer : MonoBehaviour
{
    public Camera mainCamera; // Player camera
    public float warpSpeed = 2; // Speed for warping through walls
    public Image reticle; // The player's reticle
    public Image screenTint; // The screen tint that appears in warp mode. Also displays screen effect while warping
    public Color targetColor; // Target color for screen tint
    public AudioClip warpSFX; // Warp sound - not used?

    public Sprite[] screenEffectSprites; // Sprites for warping screen effect
    public float screenEffectSpeed = 10; // Speed for warping screen effect


    private bool isWarping = false; // States whether or not player is warping
    private Vector3 destinationPosition; // Position that player is warping to
    private GameObject[] visibleWarpPoints; // List of all warp points in the scene
    private int currentSprite = 0; // Determines current sprite for warp screen effect
    private float time; // Timer for animating warp screen effect

    // Start is called before the first frame update
    void Start()
    {
        visibleWarpPoints = GameObject.FindGameObjectsWithTag("WarpPoint");
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If in warp mode
        if (Input.GetKey(KeyCode.Tab))
        {
            reticle.GetComponent<Animator>().SetBool("WarpMode", true);
            screenTint.GetComponent<Image>().color = targetColor;
            CheckForWarpPoint();
        }
        else if (!isWarping)
        {
            foreach (GameObject warpPoint in visibleWarpPoints)
            {
                DeactivateWarpPoint(warpPoint);
            }
            screenTint.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            reticle.GetComponent<Animator>().SetBool("WarpMode", false);
        }

        if (isWarping)
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            transform.position = Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * warpSpeed);
            AnimateScreenEffect();
            if (transform.position == destinationPosition)
            {
                // End warping
                isWarping = false;
                screenTint.GetComponent<Image>().sprite = null;
            }
        } 
        else
        {
            GetComponent<CharacterController>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }

    }

    // Sets warp points in scene as active
    private void CheckForWarpPoint()
    {
        foreach (GameObject warpPoint in visibleWarpPoints)
        {
            warpPoint.transform.GetChild(0).gameObject.SetActive(true);
            warpPoint.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // If colliding with win area...
        if (collider.gameObject.CompareTag("WinArea"))
        {
            Debug.Log("You have walked through a win area.");
        }
        else // Otherwise, this is probably a warp area.
        {
            if (!isWarping)
            {
                // If entering point A of warp point, go to point B
                if (collider.gameObject.transform.parent.GetChild(0).gameObject == collider.gameObject)
                {
                    destinationPosition = collider.gameObject.transform.parent.GetChild(1).transform.position;
                    collider.gameObject.GetComponent<AudioSource>().Play();
                    isWarping = true;
                }
                // If entering point B of warp point, go to point A
                else if (collider.gameObject.transform.parent.GetChild(1).gameObject == collider.gameObject)
                {
                    destinationPosition = collider.gameObject.transform.parent.GetChild(0).transform.position;
                    collider.gameObject.GetComponent<AudioSource>().Play();
                    isWarping = true;
                }

            }
        }

    }
  
    private void DeactivateWarpPoint(GameObject warpPoint)
    {
        warpPoint.transform.GetChild(0).gameObject.SetActive(false);
        warpPoint.transform.GetChild(1).gameObject.SetActive(false);
    }


    // Cycles through sprites for the screen effect that plays while player is warping
    private void AnimateScreenEffect()
    {
        screenTint.GetComponent<Image>().sprite = screenEffectSprites[currentSprite];
        if (screenEffectSpeed != 0 && time > 1.0f/ screenEffectSpeed)
        {
            currentSprite += 1;
            currentSprite %= screenEffectSprites.Length;
            time = 0;
        } 
        else
        {
            time += screenEffectSpeed * Time.deltaTime;
        }

    }
}
