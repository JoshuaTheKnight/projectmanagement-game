using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class uiManager : MonoBehaviour
{
    public gameManager gameManager;
    public movement movement;

    public int attempts = 1;

    //Creates the class to parse the JSON into. Variable names !!!!MUST!!!! coincide with the JSON Key names
    [System.Serializable]
    public class Questions
    {
        public string Question;
        public string A;
        public string B;
        public string C;
        public string D;
        public string Answer;
        public string Reason;
    }
    //Creates the wrapper for the array of Questions because JSON Utility only works with objects. Variable name !!!!MUST!!!! Coincide with the list name
    [System.Serializable]
    public class QuestionList
    {
        public Questions[] Sheet1;
    }


    void Start()
    {

    }

    //Initialize Elements
    private void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        
        //Grabs the JSON file from the resources folder and turns it into a string
        TextAsset jsonFile = Resources.Load<TextAsset>("quizQuestions");
        string jsonData = jsonFile.text;
        //Use Json Utility to turn the string into the question list
        QuestionList questionList = JsonUtility.FromJson<QuestionList>(jsonData);

        //Grabs a random question to be displayed
        System.Random rnd = new System.Random();
        int rndQuestion = rnd.Next(0, questionList.Sheet1.Length);
        Questions cur = questionList.Sheet1[rndQuestion];
       
        //Grabs the root component in order to grab the other components in the UI
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        //grab components
        Label text = root.Q<Label>("QuestionText");

        Button a = root.Q<Button>("Button1");
        Button b = root.Q<Button>("Button2");
        Button c = root.Q<Button>("Button3");
        Button d = root.Q<Button>("Button4");
        //Check if quiz has been done already
        if (attempts == 0)
        {
            text.text = "You have completed your question for the day.";
            a.text = "Exit";
            b.text = "Exit";
            c.text = "Exit";
            d.text = "Exit";
        }
        else
        {
            //Generate the question to be shown on the UI. If no attempts remaining, don't allow access
            GenerateQuestion(text, a, b, c, d, cur);
        }


        AnimatePopIn(root);
        //Set up clicked scenarios to check for answer. If attempts = 0, then button will be an exit button
        a.clicked += () =>
        {
            if (attempts == 0)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                AnimatePopOut(root);
            }
            else
            {
                checkAnswer(cur, "A", text);
                a.text = "Exit";
                b.text = "Exit";
                c.text = "Exit";
                d.text = "Exit";
            }
        };

        b.clicked += () =>
        {
            if (attempts == 0)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                AnimatePopOut(root);
            }
            else
            {
                checkAnswer(cur, "B", text);
                a.text = "Exit";
                b.text = "Exit";
                c.text = "Exit";
                d.text = "Exit";
            }
        };

        c.clicked += () =>
        {
            if (attempts == 0)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                AnimatePopOut(root);
            }
            else
            {
                checkAnswer(cur, "C", text);
                a.text = "Exit";
                b.text = "Exit";
                c.text = "Exit";
                d.text = "Exit";
            }
        };

        d.clicked += () =>
        {
            if (attempts == 0)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                AnimatePopOut(root);
            }
            else
            {
                checkAnswer(cur, "D", text);
                a.text = "Exit";
                b.text = "Exit";
                c.text = "Exit";
                d.text = "Exit";
            }
        };


    }
    //edits the text to generate the question
    private void GenerateQuestion(Label text, Button a, Button b, Button c, Button d, Questions cur)
    {
        text.text = cur.Question;
        a.text = cur.A;
        b.text = cur.B;
        c.text = cur.C;
        d.text = cur.D;
    }

    //Checks the answer
    private void checkAnswer(Questions cur, string letterAnswer, Label text)
    {
        attempts -= 1;
        if (letterAnswer == cur.Answer)
        {
            text.text = "Correct: " + cur.Reason;
            gameManager.SetRisk(5);
            gameManager.Refresh();
        }
        else
        {
            text.text = "False: " + cur.Reason;
        }
    }

    public void AnimatePopIn(VisualElement rootElement)
    {
        movement.enabled = false;
        //Get root element and perform a DoTween pop in effect
        rootElement.transform.scale = Vector3.zero;
        DOTween.To(() => rootElement.transform.scale, x => rootElement.transform.scale = x, Vector3.one, 0.5f);
    }

    public void AnimatePopOut(VisualElement rootElement)
    {
        movement.enabled = true;
        rootElement.transform.scale = Vector3.one;
        DOTween.To(() => rootElement.transform.scale, x => rootElement.transform.scale = x, Vector3.zero, 0.5f)
        .OnComplete(() => this.gameObject.SetActive(false));
    }
}
