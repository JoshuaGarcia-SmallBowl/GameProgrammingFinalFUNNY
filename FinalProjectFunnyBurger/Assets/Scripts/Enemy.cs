using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;
    public float range = 1.5f;
    
    private GameObject player;
    private Animator animator;
    void Start()
    {
        
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
       
    }

    
    void Update()
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
