using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //characteristics
    public float speed = 2;
    public int damage = 5;
    public float lifeTime = 1.2f;
    public bool split = false;
    public GameObject splitProj;
    public bool randomSpeed = false;
   
    //timer
    private float bornTime;

    //offset
    public float majorOffsetChance = 3;
    private float minorOffset = 15;
    private float majorOffset = 45;

    //external references
    private PlayerController playerController;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        //roll for an offset in angle, at a lower chance the projectile will fly in a further off direction
        if (Random.Range(1, majorOffsetChance) == 1)
        {
            transform.Rotate(0, Random.Range(-majorOffset, majorOffset), 0);
        }
        else
        {
            transform.Rotate(0, Random.Range(-minorOffset, minorOffset), 0);
            
        }
        //start the timer for when the projectile was created
        bornTime = Time.time;

        //change the projectile's speed randomly for more variety
        if (randomSpeed)
        {
            speed *= Random.Range(0.7f, 1.3f);
        }

    }

    void Update()
    {
        //movement
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        //timer
        float aliveTime = Time.time - bornTime;
        if (aliveTime > lifeTime)
        {
            if (split)
            {
                Split();

            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.takeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void Split()
    {
        //make two projectile go to the left and right of this one when it dies
        transform.Rotate(0, transform.rotation.y - 90, 0);
        Instantiate(splitProj, transform.position, transform.rotation);
        transform.Rotate(0, transform.rotation.y + 180, 0);
        Instantiate(splitProj, transform.position, transform.rotation);
    }
}
