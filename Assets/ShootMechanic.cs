using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMechanic : MonoBehaviour
{
    public ParticleSystem muzzleFlashEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            muzzleFlashEffect.Play();

            RaycastHit gunshot;
            if (Physics.Raycast(transform.position, transform.forward, out gunshot, Mathf.Infinity))
            {
                if (gunshot.collider.CompareTag("Enemy"))
                {
                    Rigidbody enemyRB = gunshot.collider.gameObject.GetComponent<Rigidbody>();
                    if (enemyRB != null)
                    {
                        if (enemyRB.constraints != RigidbodyConstraints.None)
                        {
                            enemyRB.constraints = RigidbodyConstraints.None;
                            enemyRB.AddTorque(45, 0, 0, ForceMode.Impulse);
                        }
                    }
                }
            }
        }
    }
}
