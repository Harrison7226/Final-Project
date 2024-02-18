using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayer : MonoBehaviour
{
    public Camera mainCamera;
    public float warpSpeed = 2;
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            CheckForWarpPoint();
        }
        else if (visibleWarpPoint != null)
        {
            visibleWarpPoint.GetComponent<MeshRenderer>().enabled = false;
            visibleWarpPoint = null;
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
               
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                visibleWarpPoint = hit.collider.gameObject;
            }
            else if (visibleWarpPoint != null)
            {
                visibleWarpPoint.GetComponent<MeshRenderer>().enabled = false;
                visibleWarpPoint = null;
            }
        }
        else if (visibleWarpPoint != null)
        {
            visibleWarpPoint.GetComponent<MeshRenderer>().enabled = false;
            visibleWarpPoint = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (visibleWarpPoint != null && collider.gameObject.transform == visibleWarpPoint.transform.GetChild(0))
        {
            destinationPosition = visibleWarpPoint.transform.GetChild(1).transform.position;
            isWarping = true;
        }
        else if (visibleWarpPoint != null && collider.gameObject.transform == visibleWarpPoint.transform.GetChild(1))
        {
            destinationPosition = visibleWarpPoint.transform.GetChild(0).transform.position;
            isWarping = true;
        }
    }
}
