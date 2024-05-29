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

    public int variantChance = 10;
    public bool variants = false;
    public GameObject iceVariant;
    public GameObject fireVariant;


    private bool rotatable = false;
    private bool movable = false;
    private bool hurtable = true;
    private bool attackOnCd = false;

    public bool fireImmune = false;
    public bool iceImmune = false;

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
        transform.LookAt(player.transform.position);
        //Roll if an enemy will be a fire or ice variant instead
        if (variants)
        {
            if (Random.Range(1, variantChance) == 1)
            {
                
                if (Random.Range(1, 10) <= 5)
                {
                    Instantiate(fireVariant, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
                else
                {

                    Instantiate(iceVariant, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
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
    private void OnTriggerStay(Collider other)
    {
        if (movable)
        {
            if (other.CompareTag("Player"))
            {                
                playerController.takeDamage(attack);
            }

        }


    }
    
    public void hurt(int damage, int type)
    {
       if (hurtable)
        {
            
            animator.SetBool("Damaged", true);
            if (fireImmune && iceImmune && type != 2)
            {
                damage = 0;
            }
            else if (fireImmune && type == 3)
            {
                damage /= 4;
            }
            else if (fireImmune && type == 1)
            {
                damage *= 2;
            }
            else if (iceImmune && type == 1)
            {
                damage /= 4;
            }
            else if (iceImmune && type == 3)
            {
                damage *= 2;
            }
            
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

    public void spawnAnimEnd()
    {
        movable = true;
        rotatable = true;
    }
    
}
