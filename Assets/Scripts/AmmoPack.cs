using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    [SerializeField]
    private float reloadAmount;

    [SerializeField]
    private AudioClip reloadSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            AudioSource.PlayClipAtPoint(reloadSound, transform.position);
            ShootMechanic sm = GameObject.FindObjectOfType<ShootMechanic>();
            sm.magazine += (int)reloadAmount;
            sm.UpdateUI();

            // other.GetComponent<ShootMechanic>().magazine += (int)reloadAmount;
            Destroy(gameObject);
        }
    }
}
