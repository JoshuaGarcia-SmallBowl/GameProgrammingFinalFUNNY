using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIPullUp : MonoBehaviour
{
    public GameObject targetUI;
    private bool active = false;
    public void activation()
    {
        if (!active)
        {
            targetUI.SetActive(true);
            active = true;
        }
        else
        {
            targetUI.SetActive(false);
            active = false;
        }
    }
}
