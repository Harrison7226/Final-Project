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
    private GameObject visibleWarpPoint;

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (visibleWarpPoint != null)
            {
                DeactivateWarpPoint();
            }
            screenTint.GetComponent<Image>().color = /*Color.Lerp(screenTint.GetComponent<Image>().color, */new Color(0, 0, 0, 0);//, Time.deltaTime * lerpSpeed);
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
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("WarpPoint"))
            {
                visibleWarpPoint = hit.collider.gameObject;
                visibleWarpPoint.GetComponent<MeshRenderer>().enabled = true;
                visibleWarpPoint.transform.GetChild(0).gameObject.SetActive(true);
                visibleWarpPoint.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (visibleWarpPoint != null)
            {
                DeactivateWarpPoint();
            }
        }
        else if (visibleWarpPoint != null)
        {
            DeactivateWarpPoint();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isWarping && visibleWarpPoint != null)
        {
            if (collider.gameObject.transform == visibleWarpPoint.transform.GetChild(0))
            {
                destinationPosition = visibleWarpPoint.transform.GetChild(1).transform.position;
                collider.gameObject.GetComponent<AudioSource>().Play();
                // AudioSource.PlayClipAtPoint(warpSFX, transform.position);
                isWarping = true;
            }
            else if (collider.gameObject.transform == visibleWarpPoint.transform.GetChild(1))
            {
                destinationPosition = visibleWarpPoint.transform.GetChild(0).transform.position;
                collider.gameObject.GetComponent<AudioSource>().Play();
                // AudioSource.PlayClipAtPoint(warpSFX, transform.position);
                isWarping = true;
            }
        }

    }

    private void DeactivateWarpPoint()
    {
        visibleWarpPoint.GetComponent<MeshRenderer>().enabled = false;
        visibleWarpPoint.transform.GetChild(0).gameObject.SetActive(false);
        visibleWarpPoint.transform.GetChild(1).gameObject.SetActive(false);
        visibleWarpPoint = null;
    }
}
