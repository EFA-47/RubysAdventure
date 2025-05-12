using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    Vector2 position;
    public InputAction moveAction;
    Rigidbody2D rb2D;
    Vector2 move;
    public int maxHealth = 5;
    public int currentHealth;
    public int Health {get {return currentHealth;}}
    public float movementSpeed = 6.0f;
    public float timeInvinsible = 2.0f;
    bool isInvinsible = false;
    float damageCooldown;
    public float timeHealing = 3.0f;
    bool healing = false;
    float healCooldown;
    Animator animator;
    Vector2 moveDirection = new (0,-1);
    public GameObject projectilePrefab;
    public InputAction talkAction;
    AudioSource audioSource;
    bool walking = false;
    public AudioClip hurt;
    // Start is called before the first frame update
    void Start()
    {
        moveAction.Enable();
        talkAction.Enable();

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();

        if(isInvinsible)
        {
            damageCooldown -= Time.deltaTime;
            if(damageCooldown <= 0)
            {
                isInvinsible = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if(talkAction.WasPressedThisFrame())
        {
            FindFriend();
        }

        if(healing)
        {
            healCooldown -= Time.deltaTime;
            if(healCooldown <= 0)
            {
                healing = false;
            }
        }

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x,move.y);
            moveDirection.Normalize();
            
            if(!walking)
            {
                walking = true;
                audioSource.Play();
            }
        }
        else if (walking)
        {
            walking = false;
            audioSource.Stop();
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    void FixedUpdate()
    {
        position = (Vector2) transform.position + movementSpeed * Time.deltaTime * move;
        rb2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if(isInvinsible)
            {
                return;
            }
            isInvinsible = true;
            damageCooldown = timeInvinsible;
            animator.SetTrigger("Hit");
            PlaySound(hurt);
        }
        currentHealth = Math.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth/(float)maxHealth);
    }

    public void SlowHeal(int amount)
    {
        if(healing)
        {
            return;
        }
        healing = true;
        healCooldown = timeHealing;
        currentHealth = Math.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth/(float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb2D.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        if(hit.collider != null)
        {
            NonPlayableCharacter character = hit.collider.gameObject.GetComponent<NonPlayableCharacter>();
            if(character != null)
            {
                UIHandler.instance.DisplayDialogue(character.displayText[0]);
            }
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

}
