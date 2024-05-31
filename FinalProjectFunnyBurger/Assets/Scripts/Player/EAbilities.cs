using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EAbilities : MonoBehaviour
{
    public GameObject fireAbProj;

    public GameObject iceAbProj;

    //get the player's position and the player's mouse position
    System.Collections.IEnumerator fireAbility(Vector3 aim, Vector3 playerPos)
    {
        //summon the projectiles below the floor around the mouse position
        Debug.Log("Ability Activated");
        Vector3 funny = new (aim.x, aim.y - 5, aim.z);
        //summon three seperate projectiles with equal distance between them, this distance is the offset
        Instantiate(fireAbProj, funny, Quaternion.identity);
        yield return new WaitForSeconds(0.2f); 
        Vector3 offset = playerPos - aim;
        Instantiate(fireAbProj, funny - offset, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(fireAbProj, funny - offset - offset , Quaternion.identity);
    }
        

    
    //These are functions so playerController can call them
   public void spawnProj(Vector3 target, Vector3 player)
    {
        StartCoroutine(fireAbility(target, player));
        
    }

    public void iceProj(Vector3 position, Quaternion rotation)
    {
        Instantiate(iceAbProj, position, rotation);
    }

 
}
