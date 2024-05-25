using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    public float speed = 2.0f;
    public float range = 1.5f;
    public int attack = 5;

    private bool movable = true;

    private GameObject player;
    private Animator animator;
    private PlayerController playerController;

   
    void Start()
    {
        
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();

    }

    
    void Update()
    {      
        if (movable)
        {
            //measure how far the enemy is from the player
            float distance = Vector3.Distance(transform.position, player.transform.position);

            //look torwards the player
            transform.LookAt(player.transform.position);

            //if out of range, move closer
            if (distance > range)
            {
                animator.SetBool("Attacking", false);
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else
            {
                animator.SetBool("Attacking", true);
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
        animator.SetBool("Damaged", true);
        health -= damage;

        if (health <= 0)
        {
            animator.SetBool("Dead", true);
            movable = false;
        }
        
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
