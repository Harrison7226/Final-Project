using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMechanic : MonoBehaviour
{
    public GameObject gun;
    public GameObject reloadingText;
    public AudioSource reloadAudioSource;

    public int maxBullets = 6;
    public int bullets = 6;

    private ParticleSystem muzzleFlashEffect;
    private AudioSource gunAudio;
    private bool reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlashEffect = gun.transform.Find("MuzzleFlashEffect").GetComponent<ParticleSystem>();
        gunAudio = gun.GetComponent<AudioSource>();
        reloadingText.SetActive(false);
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PrototypeGameManager.gameRunning)
        {
            if (bullets > 0)
            {
                muzzleFlashEffect.Play();
                gunAudio.Play();
                bullets--;

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

                if (bullets == 0)
                {
                    Invoke("Reload", 0.1f);
                }

            }
            else
            {
                Reload();
                
            }
        }
    }

    // Start reloading
    void Reload()
    {
        if (!reloading)
                {
                    reloading = true;
                    reloadAudioSource.Play();
                    
                    reloadingText.SetActive(true);
                    gun.SetActive(false);
                    
                    Invoke("FinishReload", 3);
                }
    }

    // Finish reloading
    void FinishReload()
    {
        bullets = maxBullets;
        reloadingText.SetActive(false);
        reloading = false;
        gun.SetActive(true);
    }
    
}
