using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpPlayer : MonoBehaviour
{
    public Camera mainCamera;
    public float warpSpeed = 2;
    public Image reticle;
    public Image screenTint;
    public Color targetColor;
    public AudioClip warpSFX;
   // public float lerpSpeed = 1;

    private bool isWarping = false;
    private Vector3 destinationPosition;
    private GameObject[] visibleWarpPoints;

    // Start is called before the first frame update
    void Start()
    {
        visibleWarpPoints = GameObject.FindGameObjectsWithTag("WarpPoint");
        foreach (GameObject go in visibleWarpPoints)
        {
            Debug.Log(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
            if (transform.position == destinationPosition)
            {
                isWarping = false;
            }
        } else
        {
            GetComponent<CharacterController>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }

    }

    private void CheckForWarpPoint()
    {
        foreach (GameObject warpPoint in visibleWarpPoints)
        {
            warpPoint.GetComponent<MeshRenderer>().enabled = true;
            warpPoint.transform.GetChild(0).gameObject.SetActive(true);
            warpPoint.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isWarping)
        {
            if (collider.gameObject.transform.parent.GetChild(0).gameObject == collider.gameObject)
            {
                destinationPosition = collider.gameObject.transform.parent.GetChild(1).transform.position;
                collider.gameObject.GetComponent<AudioSource>().Play();
                isWarping = true;
            }
            else if (collider.gameObject.transform.parent.GetChild(1).gameObject == collider.gameObject)
            {
                destinationPosition = collider.gameObject.transform.parent.GetChild(0).transform.position;
                collider.gameObject.GetComponent<AudioSource>().Play();
                isWarping = true;
            }

        }

    }

    private void DeactivateWarpPoint(GameObject warpPoint)
    {
        warpPoint.GetComponent<MeshRenderer>().enabled = false;
        warpPoint.transform.GetChild(0).gameObject.SetActive(false);
        warpPoint.transform.GetChild(1).gameObject.SetActive(false);
    }
}
