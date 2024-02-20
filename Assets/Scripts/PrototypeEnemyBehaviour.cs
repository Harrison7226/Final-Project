using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeEnemyBehaviour : MonoBehaviour
{
    public GameObject gun;

    public bool alive = true; // Still alive?
    public bool canShoot = true; // Can this character attack?
    
    private Transform targetTransform; // The player's transform. What the enemy will look at
    private ParticleSystem muzzleFlashEffect; // Gun muzzle flash effect
    private AudioSource gunAudio; // Gunshot sound effect

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = FindObjectOfType<FPSController>().gameObject.transform;
        muzzleFlashEffect = gun.transform.Find("MuzzleFlashEffect").GetComponent<ParticleSystem>();
        gunAudio = gun.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (alive)
        {
            RaycastHit enemyView;
            if (Vector3.Dot(targetTransform.position - transform.position, transform.forward) > 0
                && Physics.Raycast(transform.position, targetTransform.position - transform.position, out enemyView, Mathf.Infinity)
                && enemyView.collider.CompareTag("Player"))
            {
                transform.LookAt(targetTransform);
            }

            RaycastHit gunview;
            if (Physics.Raycast(transform.position, transform.forward, out gunview, Mathf.Infinity))
            {
                if (gunview.collider.CompareTag("Player"))
                {
                    if (canShoot)
                    {
                        canShoot = false;
                        Invoke("Shoot", 1);
                    }
                }
            }
        }
    }

    void Shoot()
    {
        if (alive)
        {
            muzzleFlashEffect.Play();
            gunAudio.Play();
            canShoot = true;

            // Actually calculate the gunshot
            RaycastHit gunshot;
            if (Physics.Raycast(transform.position, transform.forward, out gunshot, Mathf.Infinity))
            {
                if (gunshot.collider.CompareTag("Player"))
                {
                    FindObjectOfType<PrototypeGameManager>().GameOverMessage("You were shot! Game over.");
                }
            }
        }
    }

    public void Die()
    {
        alive = false;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(0, 0, -50, ForceMode.Impulse);
    }
}
