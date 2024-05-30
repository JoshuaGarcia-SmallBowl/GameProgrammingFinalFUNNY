using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
    public float heat = 0;
    private bool selecting = false;
    private float heatToChange;
    private bool heatChanging;

    public GameObject panel;
    private PlayerController playerController;
    public Slider heatBar;
    public Slider changeBar;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    System.Collections.IEnumerator changeHeat(float change)
    {
        
        heatChanging = true;
        
        for (float i = Mathf.Abs(change / 20f); i != 0; i--)
        {
            yield return new WaitForSeconds(1.5f);
            if (change < 0)
            {
                heat -= 20;
            }
            if(change > 0)
            {
                heat += 20;
            }
            Debug.Log("Heat:" + heat);
            if (heat <= 20)
            {
                Debug.Log("Frozen");
            }
            else if (heat >= 80)
            {
                Debug.Log("Burning");
            }
            else
            {
                Debug.Log("Default");
            }
            SetHeatBar();
            
        }
        heatChanging = false;
        heatToChange = 0;
        
    }


    private void Update()
    {
        if (!heatChanging)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetHeatBar();
                Time.timeScale = 0.2f;
                selecting = true;
                panel.gameObject.SetActive(true);
                playerController.movability(false);
            }
            if (selecting)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    heatToChange -= 20;
                    if (heat + heatToChange < 0)
                    {
                        heatToChange += 20;
                    }
                    else
                    {
                        Debug.Log("Heat to change: " + heatToChange);
                        SetHeatBar();
                    }
                    
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    heatToChange += 20;
                    if (heat + heatToChange > 100)
                    {
                        heatToChange -= 20;
                    }
                    else
                    {
                        Debug.Log("Heat to change: " + heatToChange);
                        SetHeatBar();
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                Time.timeScale = 1;
                
                selecting = false;
                panel.gameObject.SetActive(false);
                playerController.movability(true);
                StartCoroutine(changeHeat(heatToChange));
            }
        }
        
    }

    private void SetHeatBar()
    {
        heatBar.value = heat / 20;
        changeBar.value = heatBar.value + heatToChange / 20;
        playerController.heatChangePC(heat);
    }
}
