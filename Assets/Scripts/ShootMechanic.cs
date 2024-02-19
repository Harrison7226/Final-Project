using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMechanic : MonoBehaviour
{
    public GameObject gun;
    
    private ParticleSystem muzzleFlashEffect;
    private AudioSource gunAudio;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlashEffect = gun.transform.Find("MuzzleFlashEffect").GetComponent<ParticleSystem>();
        gunAudio = gun.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            muzzleFlashEffect.Play();
            gunAudio.Play();

            RaycastHit gunshot;
            if (Physics.Raycast(transform.position, transform.forward, out gunshot, Mathf.Infinity))
            {
                if (gunshot.collider.CompareTag("Enemy"))
                {
                    Rigidbody enemyRB = gunshot.collider.gameObject.GetComponent<Rigidbody>();
                    PrototypeEnemyBehaviour enemyEB = gunshot.collider.gameObject.GetComponent<PrototypeEnemyBehaviour>();
                    if (enemyRB != null)
                    {
                        if (enemyEB != null && enemyEB.alive)
                        {
                            enemyEB.Die();
                        }
                    }
                }
                else if (gunshot.collider.CompareTag("Civilian"))
                {
                    FindObjectOfType<PrototypeGameManager>().GameOverMessage("You shot a civilian! Game over.");
                }
            }
        }
    }
}
