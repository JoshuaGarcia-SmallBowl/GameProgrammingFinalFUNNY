using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public int offset = 19;
    void Start()
    {
        
    }

    void Update()
    {
       //keep the camera above the player
        transform.position = new(player.transform.position.x, offset, player.transform.position.z);
        
    }
}
