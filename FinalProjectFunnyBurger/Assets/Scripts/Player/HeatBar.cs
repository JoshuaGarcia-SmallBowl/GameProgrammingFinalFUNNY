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


    //object references
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
        
        //get the absolute value of the amount of heat to change then divide it by 20, this is to figure out how many times the heatBar will have to tick to reach the desired value

        for (float i = Mathf.Abs(change / 20f); i != 0; i--)
        {
            
            yield return new WaitForSeconds(1.5f);
            //move the heat by one tick either left or right if it's rasing or lowering the heat value
            if (change < 0)
            {
                heat -= 20;
            }
            else if(change > 0)
            {
                heat += 20;
            }
            
            SetHeatBar();
            
        }
        //reset the heat to change so it can work the next time
        heatChanging = false;
        heatToChange = 0;
        
    }


    private void Update()
    {
        
        if (!heatChanging)
        {
            //when tab is pressed, pull up the heat bar
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetHeatBar();
                Time.timeScale = 0.2f;
                selecting = true;
                panel.gameObject.SetActive(true);
                playerController.movability(false);
            }
            //while the heat bar is up, Q and E move the selection left and right
            if (selecting)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    heatToChange -= 20;
                    //check if this change would go below 0
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
                    //check if this change would go above 100
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
            //when tab is released, the heat bar dissapears, and the heat begins changing to the point selected
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

    //make the heat bar update to reflect the heat being changed, also tell the heat to playerController
    private void SetHeatBar()
    {
        heatBar.value = heat / 20;
        changeBar.value = heatBar.value + heatToChange / 20;
        playerController.heatChangePC(heat);
    }
}
