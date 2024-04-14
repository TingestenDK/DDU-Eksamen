using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerScript : MonoBehaviour
{
    public float laptime;
    public float besttime = 0;
    private bool startTimer;
    public float sektorTime1;
    public float sektorTime2;
    public float sektorTime3;
    public float sektorTime4;
    
    private bool checkpoint1 = false;
    private bool checkpoint2 = false;
    private bool checkpoint3 = false;
    private bool checkpoint4 = false;
 
    public UnityEngine.UI.Text lapTimeText;
    public UnityEngine.UI.Text BestTimeText;
    public UnityEngine.UI.Text sektor1Text;
    public UnityEngine.UI.Text sektor2Text;
    public UnityEngine.UI.Text sektor3Text;
    public UnityEngine.UI.Text sektor4Text;


    void Start()
    {
        
    }

    void Update()
    {
        if(startTimer == true)
        {
            laptime = laptime + Time.deltaTime;
            lapTimeText.text = "Time: " + laptime.ToString("F2");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "checkpoint1")
        {
            sektorTime1 = laptime;
            sektor1Text.text = "Sektor 1: " + sektorTime1.ToString("F2");
            checkpoint1 = true;
        }

        if (other.gameObject.name == "checkpoint2")
        {
            sektorTime2 = laptime - sektorTime1;
            sektor2Text.text = "Sektor 2: " + sektorTime2.ToString("F2");
            checkpoint2 = true;
        }

        if (other.gameObject.name == "checkpoint3")
        {
            sektorTime3 = laptime - (sektorTime1 + sektorTime2);
            sektor3Text.text = "Sektor 3: " + sektorTime3.ToString("F2");
            checkpoint3 = true;
        }

        if (other.gameObject.name == "checkpoint4")
        {
            sektorTime4 = laptime - (sektorTime1 + sektorTime2 + sektorTime3); // Calculate sektor time for the fourth segment
            sektor4Text.text = "Sektor 4: " + sektorTime4.ToString("F2");
            checkpoint4 = true;
        }
        if (other.gameObject.name == "StartFinish")
        {
            if (startTimer == false)
            {
                startTimer = true;
                laptime = 0;
                checkpoint1 = false;
                checkpoint2 = false;
                checkpoint3 = false;
                checkpoint4 = false; // New checkpoint flag
            }

            if (checkpoint1 && checkpoint2 && checkpoint3 && checkpoint4 == true) // Check for all checkpoints
            {


                if (besttime == 0)
                {
                    besttime = laptime;
                }

                if (laptime < besttime)
                {
                    besttime = laptime;
                }
                lapTimeText.text = "Laptime: " + laptime.ToString("F2");
                laptime = 0;
                
                BestTimeText.text = "Best: " + besttime.ToString("F2");
                checkpoint1 = false;
                checkpoint2 = false;
                checkpoint3 = false;
                checkpoint4 = false;
            }
        }


        
    }

}