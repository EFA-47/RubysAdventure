using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int ChangeHealth = 1;
    public AudioClip audioClip;
    public GameObject collect;
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null && player.Health < player.maxHealth)
        {
            player.ChangeHealth(ChangeHealth);
            player.PlaySound(audioClip);
            GameObject effect = Instantiate(collect, transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }

}
