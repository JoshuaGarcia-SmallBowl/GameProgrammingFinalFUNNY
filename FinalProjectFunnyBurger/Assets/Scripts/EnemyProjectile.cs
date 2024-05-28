using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 2;
    public int damage = 5;
    public float lifeTime = 1.2f;
    public bool split = false;
    public GameObject splitProj;
    public bool randomSpeed = false;
   

    private float bornTime;

    public float majorOffsetChance = 3;
    private float minorOffset = 15;
    private float majorOffset = 45;

    private PlayerController playerController;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        if (Random.Range(1, majorOffsetChance) == 1)
        {
            transform.Rotate(0, Random.Range(-majorOffset, majorOffset), 0);
        }
        else
        {
            transform.Rotate(0, Random.Range(-minorOffset, minorOffset), 0);
            
        }
        bornTime = Time.time;
        if (randomSpeed)
        {
            speed *= Random.Range(0.7f, 1.3f);
        }

    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
        transform.Rotate(0, transform.rotation.y - 90, 0);
        Instantiate(splitProj, transform.position, transform.rotation);
        transform.Rotate(0, transform.rotation.y + 180, 0);
        Instantiate(splitProj, transform.position, transform.rotation);
    }
}
