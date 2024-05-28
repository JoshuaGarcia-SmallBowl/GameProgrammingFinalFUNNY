using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    public float speed = 2.0f;
    public float range = 1.5f;
    public int attack = 5;
    public bool ranged = false;
    public GameObject projectile;
    public float attackCd = 1.7f;

    private bool rotatable = true;
    private bool movable = true;
    private bool hurtable = true;
    private bool attackOnCd = false;

    private GameObject player;
    private Animator animator;
    private PlayerController playerController;
    private BoxCollider boxCollider;
   
    void Start()
    {
        
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider>();

    }

    
    void Update()
    {      
        if (rotatable)
        {
            //look torwards the player
            transform.LookAt(player.transform.position);
        }
        if (movable)
        {
            //measure how far the enemy is from the player
            float distance = Vector3.Distance(transform.position, player.transform.position);

            

            //if out of range, move closer
            if (distance > range)
            {
                
                animator.SetBool("Attacking", false);
                
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else
            {
                
                animator.SetBool("Attacking", true);
                if (ranged)
                {
                    if (!attackOnCd)
                    {
                        StartCoroutine(shoot());
                    }
                }
                
            }
        }
               
    }
    private void OnTriggerEnter(Collider other)
    {
        if (movable)
        {
            if (other.CompareTag("Player"))
            {                
                playerController.takeDamage(attack);
            }

        }


    }
    
    public void hurt(int damage)
    {
       if (hurtable)
        {
            animator.SetBool("Damaged", true);
            health -= damage;

            if (health <= 0)
            {
                StartCoroutine(Invincibility());
                animator.SetBool("Dead", true);
                rotatable = false;
                movable = false;
                boxCollider.enabled = false;
            }
        }
        

    }

    System.Collections.IEnumerator shoot()
    {
        movable = false;
        attackOnCd = true;        
        animator.SetBool("Cooldown", true);
        yield return new WaitForSeconds(attackCd);
        animator.SetBool("Cooldown", false);
        movable = true;
        attackOnCd = false;
        
    }

    System.Collections.IEnumerator Invincibility()
    {
        boxCollider.enabled = false;
        hurtable = false;
        
        yield return new WaitForSeconds(0.3f);
        
        hurtable = true;
        boxCollider.enabled = true;
    }
    //used so the shot comes out with animation
    public void shootTime()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }
    public void damageEnd()
    {
        animator.SetBool("Damaged", false);
    }
    public void die()
    {
        Destroy(gameObject);
    }
    
}
