using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCameraScript : MonoBehaviour
{
    public Vector3 moveDirection;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += moveDirection * Time.deltaTime;
    }
}
