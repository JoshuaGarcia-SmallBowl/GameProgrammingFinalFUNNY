using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2;
    public int damage = 5;
    
    
    void Start()
    {
       
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.x < -30 || transform.position.x > 40)
        {
            Destroy(gameObject);
        }
        if (transform.position.z < -160 || transform.position.z > -30)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        
        Enemy target = other.gameObject.GetComponent<Enemy>();
        if (target != null)
        {
            target.hurt(damage);
            Destroy(gameObject);
        }
        
       
    }
        
    
    

    
}
