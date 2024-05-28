using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EAbilities : MonoBehaviour
{
    public GameObject fireAbProj;

    public GameObject iceAbProj;
    System.Collections.IEnumerator fireAbility(Vector3 aim, Vector3 playerPos)
    {
        
        Debug.Log("Ability Activated");
        Vector3 funny = new (aim.x, aim.y - 10, aim.z);
        Instantiate(fireAbProj, funny, Quaternion.identity);
        yield return new WaitForSeconds(0.2f); 
        Vector3 offset = playerPos - aim;
        Instantiate(fireAbProj, funny - offset, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(fireAbProj, funny - offset - offset , Quaternion.identity);
    }
        

    

   public void spawnProj(Vector3 target, Vector3 player)
    {
        StartCoroutine(fireAbility(target, player));
        
    }

    public void iceProj(Vector3 position, Quaternion rotation)
    {
        Instantiate(iceAbProj, position, rotation);
    }

 
}
