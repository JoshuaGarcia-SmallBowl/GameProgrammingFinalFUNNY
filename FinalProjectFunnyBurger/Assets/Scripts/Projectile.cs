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
