
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;
    private int damage = 1;
    public float movementSpeed;
    Vector2 position;
    private Vector2 move = new(0,1);
    float moveTimer = 3;
    float timer = 0;
    public bool horizontal;
    Animator animator;
    AudioSource audioSource;
    bool broken = true;
    public AudioClip fix;
    public ParticleSystem smoke;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //smoke = GetComponent<ParticleSystem>();

        //horizontal = Random.Range(0,2) > 0;
        if(horizontal)
        {
            move = new(1,0);
        }
        animator.SetFloat("Move X", move[0] * movementSpeed);
        animator.SetFloat("Move Y", move[1] * movementSpeed);
    }

    void Update()
    {
        MoveTimer();
    }

    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }
        position = (Vector2) transform.position + movementSpeed * Time.deltaTime * move;
        rb2D.MovePosition(position);
    }

    public void Fix()
    {
        broken = false;
        rb2D.simulated = false;
        audioSource.Stop();
        animator.SetTrigger("Fixed");
        audioSource.PlayOneShot(fix);
        smoke.Stop();
    }

    void MoveTimer()
    {
        timer += Time.deltaTime;
        if(timer> moveTimer)
        {
            movementSpeed *=-1;
            timer =0;

            if(Random.Range(0,2) > 0)
            {
                ChangeAxis();
            }
            animator.SetFloat("Move X", move[0] * movementSpeed);
            animator.SetFloat("Move Y", move[1] * movementSpeed);
        }
    }

    void ChangeAxis()
    {

        
        if(horizontal)
        {
            move = new(0,1);
        }
        else{
            move = new(1,0);
        }
        horizontal = !horizontal;
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if(!broken)
            return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            player.ChangeHealth(-damage);
        }
    }
}
