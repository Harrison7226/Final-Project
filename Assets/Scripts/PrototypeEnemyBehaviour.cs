using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeEnemyBehaviour : MonoBehaviour
{
    public GameObject gun;

    public bool alive = true; // Still alive?
    public bool canShoot = true; // Can this character attack?
    public bool riggedEnemy = false; // Rigged enemy or Capsule enemy?
    public float gunHeight = 0f; // States gun height for ray-tracing purposes. 2f works well for rigged enemies
    
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

                // transform.LookAt(targetTransform); : replaced with code below
               
                // This is some code I used in another project that prevents the enemies from
                // looking upwards when they face the player
                Vector3 targetPosition = enemyView.collider.gameObject.transform.position;
                targetPosition.y = transform.position.y;
                transform.Rotate(0, Vector3.SignedAngle((targetPosition - transform.position).normalized, transform.forward, Vector3.down), 0);


                if (GetComponent<Animator>() != null)
                {
                    GetComponent<Animator>().SetInteger("AnimState", 1);
                }

            } else
            {
                if (GetComponent<Animator>() != null)
                {
                    GetComponent<Animator>().SetInteger("AnimState", 0);
                }
            }

            RaycastHit gunview;
            if (Physics.Raycast(transform.position + new Vector3(0, gunHeight, 0), transform.forward, out gunview, Mathf.Infinity))
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
            if (Physics.Raycast(transform.position + new Vector3(0, gunHeight, 0), transform.forward, out gunshot, Mathf.Infinity))
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



        if (!riggedEnemy)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.AddTorque(0, 0, -50, ForceMode.Impulse);
        }
        else
        {
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetInteger("AnimState", 2);
            }
        }

        Destroy(gameObject, 2);

    }
}
