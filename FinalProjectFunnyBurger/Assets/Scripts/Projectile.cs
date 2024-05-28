using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2;
    public int damage = 5;
    public int health = 2;
    public float lifeTime = 1.2f;
    private float bornTime;

    public bool split = false;
    public GameObject splitProj;
    public bool upFly;

    void Start()
    {
        bornTime = Time.time;
    }

    void Update()
    {
        if (upFly)
        {
            transform.Translate(0, 0.2f * Time.deltaTime, 0);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
        if (transform.position.x < -30 || transform.position.x > 40)
        {
            Destroy(gameObject);
        }
        if (transform.position.z < -160 || transform.position.z > -30)
        {
            Destroy(gameObject);
        }
        float aliveTime = Time.time - bornTime;
        if (aliveTime > lifeTime)
        {
            DestroyH();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        
        Enemy target = other.gameObject.GetComponent<Enemy>();
        if (target != null)
        {
            target.hurt(damage);
            health--;
            if (health == 0)
            {
                DestroyH();
            }
            
        }      
    }
    private void DestroyH()
    {
        if (split)
        {
            Split();
        }
        Destroy(gameObject);
    }
    private void Split()
    {
        Quaternion original = transform.rotation;
        for (float i = -45; i <= 45; i += 15)
        {
            transform.Rotate(0, i, 0);
            Instantiate(splitProj, transform.position, transform.rotation);
            transform.rotation = original;
        }
        
        
    }





}
