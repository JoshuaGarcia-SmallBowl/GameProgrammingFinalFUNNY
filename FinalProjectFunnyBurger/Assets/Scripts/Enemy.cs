using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;
    
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position);
        transform.LookAt(player.transform.position);
        lookDirection = lookDirection.normalized;
        transform.Translate(lookDirection.x * Time.deltaTime * speed, 0f, lookDirection.z * Time.deltaTime * speed);
        
    }
}
