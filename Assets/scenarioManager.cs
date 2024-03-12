using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class scenarioManager : MonoBehaviour
{
    public gameManager gameManager;
    public movement movement;

    public int attempts = 3;

    [System.Serializable]
    public class Scenarios
    {
        public string Scenario;
        public string Option1;
        public int Cost;
        public int Scope;
        public int Time;
        public int Happiness;
        public int Risk;
        public string Option2;
        public int Cost2;
        public int Scope2;
        public int Time2;
        public int Happiness2;
        public int Risk2;
    }
    //Creates the wrapper for the array of Questions because JSON Utility only works with objects. Variable name !!!!MUST!!!! Coincide with the list name
    [System.Serializable]
    public class ScenarioList
    {
        public Scenarios[] Sheet1;
    }


    private void OnEnable()
    {

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        
        //Grabs the JSON file from the resources folder and turns it into a string
        TextAsset jsonFile = Resources.Load<TextAsset>("scenario");
        string jsonData = jsonFile.text;
        //Use Json Utility to turn the string into the question list
        ScenarioList scenarioList = JsonUtility.FromJson<ScenarioList>(jsonData);

        System.Random rnd = new System.Random();
        int rndScenario = rnd.Next(0, scenarioList.Sheet1.Length);
        //Initialize elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Label text = root.Q<Label>("ScenarioText");


        Button yes = root.Q<Button>("ButtonYes");
        Button no = root.Q<Button>("ButtonNo");

        //Check to see if scenarios are completed
        CheckAttempts(yes, no, text, scenarioList.Sheet1[rndScenario]);
        AnimatePopIn(root);


        yes.clicked += () =>
        {
            //If there are no more attempts remaining, leave
            if (yes.text == "Exit")
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                AnimatePopOut(root);
            }
            //Change button text then check the answer
            else
            {
                yes.SetEnabled(false);
                no.text = "next";
                CheckAnswer(yes, text, scenarioList.Sheet1[rndScenario]);
            }
        };

        no.clicked += () =>
        {
            //If no attempts remaining, leave
            if (no.text == "Exit")
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                AnimatePopOut(root);
            }
            //If The button text is set to no, then change the text to next and check the answer
            else if (no.text == "No")
            {
                yes.SetEnabled(false);
                no.text = "next";
                CheckAnswer(no, text, scenarioList.Sheet1[rndScenario]);
            }
            //If the button text is set to next, then set the text to no and generate a new scenario
            else
            {
                yes.SetEnabled(true);
                rndScenario = rnd.Next(0, scenarioList.Sheet1.Length);
                no.text = "No";
                CheckAttempts(yes, no, text, scenarioList.Sheet1[rndScenario]);
            }

        };
    }

    //Generates the text for new scenario.
    private void GenerateScenario(Label text, string scenario)
    {
        text.text = scenario;
    }

    //Checks the answer, reduces attempts, sets the new values based on the consequences
    private void CheckAnswer(Button btn, Label text, Scenarios scenario)
    {
        attempts -= 1;
        if (btn.text == "Yes")
        {
            text.text = "You said " + btn.text + ", here are the consequences \n" + "cost: " + scenario.Cost + "  scope: " + scenario.Scope + "  time: " + scenario.Time + "  happiness: " + scenario.Happiness + "  risk: " + scenario.Risk + "\nYou have " + attempts + " more scenarios to attend to.";
            gameManager.SetCost(scenario.Cost);
            gameManager.SetScope(scenario.Scope);
            gameManager.SetTime(scenario.Time);
            gameManager.SetHappiness(scenario.Happiness);
            gameManager.SetRisk(scenario.Risk);
        }else{
            text.text = "You said " + btn.text + ", here are the consequences \n" + "cost: " + scenario.Cost2 + "  scope: " + scenario.Scope2 + "  time: " + scenario.Time2 + "  happiness: " + scenario.Happiness2 + "  risk: " + scenario.Risk2 + "\nYou have " + attempts + " more scenarios to attend to.";
            gameManager.SetCost(scenario.Cost2);
            gameManager.SetScope(scenario.Scope2);
            gameManager.SetTime(scenario.Time2);
            gameManager.SetHappiness(scenario.Happiness2);
            gameManager.SetRisk(scenario.Risk2);
        }
        gameManager.checkLoss();
        gameManager.Refresh();
    }

    private void CheckAttempts(Button yes, Button no, Label text, Scenarios scenario)
    {
        //If there are no attempts, Set up for exit, else generate scenario
        if (attempts == 0)
        {
            yes.text = "Exit";
            no.text = "Exit";
            text.text = "You have no more situations to attend to today. Come back tomorrow!";
        }
        else
        {
            GenerateScenario(text, scenario.Scenario);
        }
    }

    public void AnimatePopIn(VisualElement rootElement)
    {
        //Sets the scale to zero, then does a .5 second pop in from 0 scale to 1
        rootElement.transform.scale = Vector3.zero;
        DOTween.To(() => rootElement.transform.scale, x => rootElement.transform.scale = x, Vector3.one, 0.5f);
    }

    public void AnimatePopOut(VisualElement rootElement)
    {
        movement.enabled = true;
        //Set scale to one, then do a .5 second pop out from 1 scale to 0, disable element on completion
        rootElement.transform.scale = Vector3.one;
        DOTween.To(() => rootElement.transform.scale, x => rootElement.transform.scale = x, Vector3.zero, 0.5f)
        .OnComplete(() => this.gameObject.SetActive(false));
    }
}
