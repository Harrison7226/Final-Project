using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    [SerializeField]
    private float reloadAmount;

    [SerializeField]
    private AudioClip reloadSound;

    public void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            AudioSource.PlayClipAtPoint(reloadSound, transform.position);
            Destroy(gameObject);
        }
    }
}
