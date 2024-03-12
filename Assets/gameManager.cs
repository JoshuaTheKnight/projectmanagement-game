using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    //initialize variables
    public int cost = 30;
    public int scope = 30;
    public int time = 30;
    public int happiness = 30;
    public int risk = 30;
    public TMP_Text scoreboard;
    public GameObject scenarioInterface;
    public GameObject endGameInterface;
    public GameObject tutorialInterface;
    public GameObject player;
    public movement movement;
    public uiManager quizManager;
    public scenarioManager scenarioManager;
    

    // Display values on the scoreboard
    void Start()
    {
        scoreboard.text = "Cost     Scope     Time      Happiness      Risk \n"
                        + cost + "         " + scope + "           " + time + "            " + happiness + "                 " + risk;
    
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tutorialInterface.SetActive(true);
    }

    // refresh scoreboard
    public void Refresh()
    {
        scoreboard.text = "Cost     Scope     Time      Happiness      Risk \n"
                        + cost + "         " + scope + "           " + time + "            " + happiness + "                 " + risk;
    }

    public void checkLoss()
    {
        //if any value is 0, initiate end game interface
        if (cost <= 0 || scope <= 0 || time <= 0 || happiness <= 0 || risk <= 0)
        {
            scenarioInterface.SetActive(false);
            endGameInterface.SetActive(true);
        }
    }

    public void Restart()
    {
        //Reset scores, refresh board, refresh attempts, and reposition player. Initiate tutorial
        cost = 30;
        scope = 30;
        time = 30;
        happiness = 30;
        risk = 30;
        Refresh();
        quizManager.attempts = 1;
        scenarioManager.attempts = 3;
        movement.enabled = true;
        player.transform.position = new Vector3(0, 7, -45);
        player.transform.rotation = Quaternion.Euler(0, 270, 0);
        tutorialInterface.SetActive(true);
    }

    public void SetCost(int value)
    {
        if ((cost + value) >= 30)
        {
            cost = 30;
        }
        else
        {
            cost += value;
        }
    }

    public void SetScope(int value)
    {
        if ((scope + value) >= 30)
        {
            scope = 30;
        }
        else
        {
            scope += value;
        }
    }

    public void SetTime(int value)
    {
        if ((time + value) >= 30)
        {
            time = 30;
        }
        else
        {
            time += value;
        }
    }

    public void SetHappiness(int value)
    {
        if ((happiness + value) >= 30)
        {
            happiness = 30;
        }
        else
        {
            happiness += value;
        }
    }

    public void SetRisk(int value)
    {
        if ((risk + value) >= 30)
        {
            risk = 30;
        }
        else
        {
            risk += value;
        }
    }
}
