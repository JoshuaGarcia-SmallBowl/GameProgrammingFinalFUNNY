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
    //1 for frozen, 2 for normal, 3 for burning
    public int type = 2;

    private float bornTime;

    public bool split = false;
    public GameObject splitProj;
    public bool upFly = false;
    private Rigidbody rb;

    public bool particle;
    public GameObject particlePrefab;

    void Start()
    {
        bornTime = Time.time;
        if (upFly)
        {
            Debug.Log("detected");
            rb = GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (!upFly)
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
        if (split)
        {
            Split();
        }
        if (particle)
        {
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
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
