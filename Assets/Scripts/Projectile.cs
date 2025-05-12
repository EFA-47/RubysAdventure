using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb2D;
    PlayerController player;
    float destroyTimer = 4;
    public AudioClip projectileThrow;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();

    }
    // Start is called before the first frame update
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) > 100.0f)
        {
            Destroy(gameObject);
        }

        destroyTimer-= Time.deltaTime;
        if(destroyTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rb2D.AddForce(direction * force);
        player.PlaySound(projectileThrow);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if(enemy != null)
        {
            enemy.Fix();
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
