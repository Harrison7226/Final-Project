using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShootMechanic : MonoBehaviour
{
    public GameObject gun;
    public GameObject reloadingText;
    public AudioSource reloadAudioSource;
    public AudioSource emptyAudioSource;

    public int maxBullets = 6;
    public int bullets = 6;
    public int magazine = 30;

    private ParticleSystem muzzleFlashEffect;
    private AudioSource gunAudio;
    private bool reloading = false;

    public Image reticleImage;
    public Color enemyColor;
    public Color civilianColor;

    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI magazineText;

    Color originalReticleColor;

    void Start()
    {
        muzzleFlashEffect = gun.transform.Find("MuzzleFlashEffect").GetComponent<ParticleSystem>();
        gunAudio = gun.GetComponent<AudioSource>();
        reloadingText.SetActive(false);
        reloading = false;

        originalReticleColor = reticleImage.color;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PrototypeGameManager.gameRunning && !reloading)
        {
            if (bullets > 0)
            {
                Shoot();
            }
            else if (bullets == 0 && magazine == 0)
            {
                emptyAudioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && bullets < maxBullets && magazine > 0)
        {
            Reload();
        }
    }

    void FixedUpdate()
    {
        ReticleEffect();
    }

    void Shoot()
    {
        muzzleFlashEffect.Play();
        gunAudio.Play();
        bullets--;
        UpdateUI();

        RaycastHit gunshot;
        if (Physics.Raycast(transform.position, transform.forward, out gunshot, Mathf.Infinity))
        {
            if (gunshot.collider.CompareTag("Enemy"))
            {
                Rigidbody enemyRB = gunshot.collider.gameObject.GetComponent<Rigidbody>();
                PrototypeEnemyBehaviour enemyEB = gunshot.collider.gameObject.GetComponent<PrototypeEnemyBehaviour>();
                if (enemyRB != null && enemyEB != null && enemyEB.alive)
                {
                    enemyEB.Die();
                }
            }
            else if (gunshot.collider.CompareTag("Civilian"))
            {
                FindObjectOfType<PrototypeGameManager>().GameOverMessage("You shot a civilian! Game over.");
            }
        }

        if (bullets == 0 && magazine > 0)
        {
            Invoke("Reload", 0.1f);
        }
    }

    void Reload()
    {
        if (!reloading)
        {
            reloading = true;
            reloadAudioSource.Play();

            reloadingText.SetActive(true);
            gun.SetActive(false);

            int bulletsToReload = maxBullets - bullets;
            if (bulletsToReload > magazine)
            {
                bullets += magazine;
                magazine = 0;
            }
            else
            {
                bullets += bulletsToReload;
                magazine -= bulletsToReload;
            }

            Invoke("FinishReload", 3);
        }
    }

    void FinishReload()
    {
        reloadingText.SetActive(false);
        reloading = false;
        gun.SetActive(true);
        UpdateUI();
    }

    void ReticleEffect()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                reticleImage.color = Color.Lerp(reticleImage.color, enemyColor, Time.deltaTime * 2);
                reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else if (hit.collider.CompareTag("Civilian"))
            {
                reticleImage.color = Color.Lerp(reticleImage.color, civilianColor, Time.deltaTime * 2);
                reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else
            {
                reticleImage.color = Color.Lerp(reticleImage.color, originalReticleColor, Time.deltaTime * 2);
                reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * 2);
            }
        }
        else
        {
            reticleImage.color = Color.Lerp(reticleImage.color, originalReticleColor, Time.deltaTime * 2);
            reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * 2);
        }
    }

    void UpdateUI()
    {
        bulletsText.SetText(bullets.ToString() + "/");
        magazineText.SetText(magazine.ToString());
    }
}
