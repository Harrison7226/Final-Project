using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrototypeEnemyBehaviour : MonoBehaviour
{
    public enum FSMStates
    {
        Patrol,
        Chase,
        Attack
    }

    public GameObject gun;

    public bool alive = true; // Still alive?
    public bool canShoot = true; // Can this character attack?
    public bool riggedEnemy = false; // Rigged enemy or Capsule enemy?
    public float gunHeight = 0f; // States gun height for ray-tracing purposes. 2f works well for rigged enemies
    public float shootDelay = 1f; // States how long enemies should wait before shooting
    public GameObject eyes;
    
    private Transform targetTransform; // The player's transform. What the enemy will look at
    private ParticleSystem muzzleFlashEffect; // Gun muzzle flash effect
    private AudioSource gunAudio; // Gunshot sound effect
    private bool alerted;

    public bool smartAgent = false; // Is this a smart AI or will it stay in place?
    public Transform[] wanderPoints; // List of all wander points.
    private int wanderPointIndex = 0; // Index of current wander point.
    private NavMeshAgent agent;
    private Vector3 nextDestination; // Next place to wander to
    private FSMStates currentState = FSMStates.Patrol;

    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        nextDestination = transform.position;

        targetTransform = FindObjectOfType<FPSController>().gameObject.transform;
        muzzleFlashEffect = gun.transform.Find("MuzzleFlashEffect").GetComponent<ParticleSystem>();
        gunAudio = gun.GetComponent<AudioSource>();
        alerted = false;

        if (smartAgent)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(nextDestination);
        }
    }

    // Called once every patrol physics update
    void UpdatePatrol()
    {
        if (smartAgent)
        {
            GetComponent<Animator>().SetInteger("AnimState", 0);
            agent.speed = 3.5f;
            agent.stoppingDistance = 0;

            FaceTarget(nextDestination);

            if (Vector3.Distance(nextDestination, transform.position) < 1f)
            {
                wanderPointIndex++;
                if (wanderPointIndex > wanderPoints.Length - 1)
                {
                    wanderPointIndex = 0;
                }
                nextDestination = wanderPoints[wanderPointIndex].position;

                agent.SetDestination(nextDestination);
            }
        }


    }

    void UpdateChase()
    {
        if (smartAgent)
        {
            Debug.Log("Chasing!");
            GetComponent<Animator>().SetInteger("AnimState", 3);
            agent.speed = 6;
            agent.stoppingDistance = 10;
            nextDestination = targetTransform.position;

            FaceTarget(nextDestination);

            if (Vector3.Distance(nextDestination, transform.position) > 30f)
            {
                Debug.Log("I lost the player. Returning to patrol behaviour.");
                currentState = FSMStates.Patrol;
                nextDestination = wanderPoints[wanderPointIndex].position;
            } else if (Vector3.Distance(nextDestination, transform.position) <= 10)
            {
                currentState = FSMStates.Attack;
            }

            agent.SetDestination(nextDestination);
        }
    }

    void UpdateAttack()
    {
        if (smartAgent)
        {
            Debug.Log("Attacking!");
            GetComponent<Animator>().SetInteger("AnimState", 1);
            agent.speed = 0;
            agent.stoppingDistance = 10;
            nextDestination = targetTransform.position;

            FaceTarget(nextDestination);

            if (Vector3.Distance(nextDestination, transform.position) > 30f)
            {
                Debug.Log("I lost the player. Returning to patrol behaviour.");
                currentState = FSMStates.Patrol;
                nextDestination = wanderPoints[wanderPointIndex].position;
            } else if (Vector3.Distance(nextDestination, transform.position) >= 15f)
            {
                currentState = FSMStates.Chase;
            }

            agent.SetDestination(nextDestination);
        }
    }

    void FixedUpdate()
    {
        if (alive)
        {
            switch (currentState)
            {
                case FSMStates.Patrol:
                    UpdatePatrol();
                    break;
                case FSMStates.Attack:
                    UpdateAttack();
                    break;
                case FSMStates.Chase:
                    UpdateChase();
                    break;
            }
            

            RaycastHit enemyView;
            if ((Vector3.Dot(targetTransform.position - transform.position, transform.forward) > 0 || alerted)
                && Physics.Raycast(eyes.transform.position, targetTransform.position - eyes.transform.position, out enemyView, Mathf.Infinity)
                && enemyView.collider.CompareTag("Player"))
            {
                Debug.Log("Player spotted!");

                FaceTarget(enemyView.collider.gameObject.transform.position);

                if (currentState == FSMStates.Patrol)
                {
                    currentState = FSMStates.Chase;
                }


                if (!smartAgent)
                {
                    GetComponent<Animator>().SetInteger("AnimState", 1);
                }

            } else
            {
                if (!smartAgent)
                {
                    GetComponent<Animator>().SetInteger("AnimState", 0);
                } 
                else if (FSMStates.Attack == currentState)
                {
                    currentState = FSMStates.Chase;
                }

            }

            RaycastHit gunview;
            if (Physics.Raycast(transform.position + new Vector3(0, gunHeight, 0), transform.forward, out gunview, Mathf.Infinity))
            {
                if (gunview.collider.CompareTag("Player"))
                {
                    Debug.Log("Player is in my sights!");
                    if (canShoot)
                    {
                        canShoot = false;
                        shootTimer = 0;
                    }
                    else
                    {
                        if (shootTimer >= shootDelay)
                        {
                            shootTimer = 0;
                            canShoot = true;
                            Shoot();
                        } else
                        {
                            shootTimer += Time.deltaTime;
                        }
                    }
                } else
                {
                    shootTimer = 0;
                    canShoot = true;
                }
            } else
            {
                shootTimer = 0;
                canShoot = true;
            }

            Debug.DrawRay(transform.position + new Vector3(0, gunHeight, 0), transform.forward * 20, Color.blue);
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 targetPosition = destination;
        targetPosition.y = transform.position.y;
        if (Mathf.Abs(Vector3.SignedAngle((targetPosition - transform.position).normalized, transform.forward, Vector3.down)) < 10 || smartAgent)
        {
            transform.Rotate(0, Vector3.SignedAngle((targetPosition - transform.position).normalized, transform.forward, Vector3.down), 0);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.FromToRotation(transform.forward, (targetPosition - transform.position).normalized), Time.deltaTime * 5);
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
                Debug.Log(gunshot.collider.name);
                if (gunshot.collider.CompareTag("Player") || gunshot.collider.CompareTag("Gun"))
                {
                    FindObjectOfType<PlayerHealth>().TakeDamage(25);
                }
            }
        }
    }

    public void Die()
    {
        if (smartAgent)
        {
            agent.speed = 0;
        }
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
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Animator>().SetInteger("AnimState", 2);
            }
        }

        Destroy(gameObject, 2);

    }

    public void OnAlert()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, targetTransform.position - eyes.transform.position, out hit, Mathf.Infinity, ~6)
             && hit.collider.CompareTag("Player"))
        {
            alerted = true;
            Invoke("OffAlert", 3);
        } else
        {
            alerted = false;
        }

    }

    void OffAlert()
    {
        alerted = false;
    }
}
