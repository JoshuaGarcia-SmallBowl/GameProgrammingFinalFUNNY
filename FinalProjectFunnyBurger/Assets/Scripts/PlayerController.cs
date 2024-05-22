using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.maxLinearVelocity = speed;
        
    }


    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(sideInput * speed, 0, forwardInput * speed); 
        
    }


}
