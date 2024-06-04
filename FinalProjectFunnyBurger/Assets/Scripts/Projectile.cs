using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //characteristics
    public float speed = 2;
    public int damage = 5;
    public int health = 2;
    public float lifeTime = 1.2f;
    //1 for frozen, 2 for normal, 3 for burning
    public int type = 2;
    public bool split = false;
    public GameObject splitProj;
    public bool upFly = false;
    private Rigidbody rb;

    //timer
    private float bornTime;

    void Start()
    {
        //start lifetime timer
        bornTime = Time.time;

        //make the proectile fly upwards instead
        if (upFly)
        {
            Debug.Log("detected");
            rb = GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    void Update()
    {
        //movement
        if (!upFly)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        //destroy when the timer ends
        float aliveTime = Time.time - bornTime;
        if (aliveTime > lifeTime)
        {
            DestroyH();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        //deal damage according to the projectile's type
        Enemy target = other.gameObject.GetComponent<Enemy>();
        if (target != null)
        {      
            target.hurt(damage, type);
            health--;
            if (health == 0)
            {
                DestroyH();
            }
            
        }      
    }
    private void DestroyH()
    {
        //Destroy projectile
        if (split)
        {
            Split();
        }
        Destroy(gameObject);
    }

    private void Split()
    {
        Quaternion original = transform.rotation;
        //Make a projectile every 15 degrees from -45 the original angle to +45
        for (float i = -45; i <= 45; i += 15)
        {
            transform.Rotate(0, i, 0);
            Instantiate(splitProj, transform.position, transform.rotation);
            transform.rotation = original;
        }
        
        
    }





}
